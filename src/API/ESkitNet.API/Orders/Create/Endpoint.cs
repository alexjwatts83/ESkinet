using ESkitNet.API.Orders.Dtos;
using ESkitNet.Core.Extensions;
using ESkitNet.Core.Services;
using FluentValidation;

namespace ESkitNet.API.Orders.Create;

public static class Endpoint
{
    public record Request(OrderDto OrderDto);
    public record Response(Order Order);

    public static async Task<IResult> Handle(Request request, ISender sender)
    {
        var command = request.Adapt<Command>();

        var result = await sender.Send(command);

        var response = result.Adapt<Response>();

        // I really don't like returning an object but in order to get things
        // going and to get through the training course I will do what they 
        // do in the course, would prefer to return CreateAt instead of Ok

        return Results.Ok(response.Order);
    }

    public record Command(OrderDto OrderDto) : ICommand<Result>;
    public record Result(Order Order);

    protected class AddressValidator : AbstractValidator<ShippingAddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name can not be null");
            RuleFor(x => x.Line1).NotEmpty().NotNull().WithMessage("Line1 can not be null");
            RuleFor(x => x.City).NotEmpty().NotNull().WithMessage("City can not be null");
            RuleFor(x => x.State).NotEmpty().NotNull().WithMessage("State can not be null");
            RuleFor(x => x.PostalCode).NotEmpty().NotNull().WithMessage("PostalCode can not be null");
            RuleFor(x => x.Country).NotEmpty().NotNull().WithMessage("Country can not be null");
        }
    }

    protected class PaymentSummaryValidator : AbstractValidator<PaymentSummaryDto>
    {
        public PaymentSummaryValidator()
        {
            RuleFor(x => x.Last4).GreaterThan(0).LessThanOrEqualTo(9999).WithMessage("Last4 is not valid");
            RuleFor(x => x.Brand).NotEmpty().NotNull().WithMessage("Brand can not be null");
            RuleFor(x => x.ExpMonth).GreaterThanOrEqualTo(1).LessThanOrEqualTo(12).WithMessage("ExpMonth is not valid");
            RuleFor(x => x.ExpYear).GreaterThanOrEqualTo(2025).LessThanOrEqualTo(2055).WithMessage("ExpYear is not valid");
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.OrderDto.CartId).NotEmpty().NotNull().WithMessage("CartId can not be null");
            RuleFor(x => x.OrderDto.ShippingAddress).NotEmpty().NotNull().WithMessage("ShippingAddress cannot be empty");
            RuleFor(x => x.OrderDto.ShippingAddress).SetValidator(new AddressValidator());
            RuleFor(x => x.OrderDto.DeliveryMethodId).NotEmpty().NotNull().WithMessage("DeliveryMethodId cannot be empty");
            RuleFor(x => x.OrderDto.PaymentSummary).SetValidator(new PaymentSummaryValidator());
        }
    }

    public class Handler(IHttpContextAccessor context, IShoppingCartService cartService, IUnitOfWork unitOfWork, IAppTimeProvider timeProvider) 
        : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var claimsPrincipal = context.HttpContext!.User;
            var email = claimsPrincipal.GetEmail(true);
            if (string.IsNullOrWhiteSpace(email))
                throw new BadHttpRequestException("Email not found for user");

            var dto = command.OrderDto;
            var cart = await cartService.GetAsync(dto.CartId, cancellationToken);

            if (cart == null)
                throw new BadHttpRequestException("Cart not found");

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
                throw new BadHttpRequestException("No Payment Intent found for Order");

            var items = await MapCartItemsToOrderItems(cart.Items, cancellationToken);

            var deliveryMethod = await unitOfWork
                .Repository<DeliveryMethod, DeliveryMethodId>()
                .GetByIdAsync(DeliveryMethodId.Of(dto.DeliveryMethodId), cancellationToken);

            if (deliveryMethod == null)
                throw new BadHttpRequestException("Delivery Method not found");

            var order = new Order(items) {
                Id = OrderId.Of(Guid.NewGuid()),
                OrderDate = timeProvider.Now,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = dto.ShippingAddress.Adapt<ShippingAddress>(),
                SubTotal = items.Sum(x => x.Quantity * x.Price),
                PaymentSummary = dto.PaymentSummary.Adapt<PaymentSummary>(),
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEmail = email
            };

            unitOfWork.Repository<Order, OrderId>().Add(order);

            var completed = await unitOfWork.Complete(cancellationToken);

            if (!completed)
                throw new BadHttpRequestException("Problem creating Order");

            return new Result(order);
        }

        private async Task<List<OrderItem>> MapCartItemsToOrderItems(List<ShoppingCartItem> cartItems, CancellationToken cancellationToken)
        {
            var items = new List<OrderItem>();

            foreach (var item in cartItems)
            {
                var productId = ProductId.Of(item.ProductId);
                var productItem = await unitOfWork.Repository<Product, ProductId>().GetByIdAsync(productId, cancellationToken) 
                    ?? throw new BadHttpRequestException("Problem with an item in the cart");
                var itemOrdered = productItem.Adapt<ProductItemOrdered>();
                var orderItem = new OrderItem
                {
                    Id = OrderItemId.Of(Guid.NewGuid()),
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
            }

            return items;
        }
    }
}
