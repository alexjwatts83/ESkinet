using FluentValidation;

namespace ESkitNet.API.Cart.Create;

public static class Endpoint
{
    public record Request(ShoppingCart Cart);
    public record Response(string Id);

    public static async Task<IResult> Handle(Request request, ISender sender)
    {
        var command = request.Adapt<Command>();

        var result = await sender.Send(command);

        var response = result.Adapt<Response>();

        return Results.Created($"/api/cart/{response.Id}", response);
    }

    public record Command(ShoppingCart Cart) : ICommand<Result>;
    public record Result(string Id);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Cart.Id).NotEmpty().NotNull().WithMessage("Id can not be null");
            RuleFor(x => x.Cart.Items).NotEmpty().WithMessage("Items cannot be empty");
            //RuleFor(x => x.Product.Name).MaximumLength(100).WithMessage("Name must be less than 100 characters");
            //RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description cannot be empty");
            //RuleFor(x => x.Product.Description).MaximumLength(1000).WithMessage("Description must be less than 1000 characters");
            //RuleFor(x => x.Product.PictureUrl).NotEmpty().WithMessage("PictureUrl cannot be empty");
            //RuleFor(x => x.Product.Type).NotEmpty().WithMessage("Type cannot be empty");
            //RuleFor(x => x.Product.Brand).NotEmpty().WithMessage("Brand cannot be empty");
            //RuleFor(x => x.Product.QuantityInStock).NotEmpty().WithMessage("QuantityInStock cannot be empty");
            //RuleFor(x => x.Product.QuantityInStock).GreaterThanOrEqualTo(0).WithMessage("QuantityInStock must be greater than or equal to 0");
            //RuleFor(x => x.Product.Price).NotEmpty().WithMessage("Price cannot be empty");
            //RuleFor(x => x.Product.Price).GreaterThanOrEqualTo(0).WithMessage("QuantityInStock must be greater than or equal to 0");
        }
    }

    public class Handler(IShoppingCartService cartService) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var cart = await cartService.SetAsync(command.Cart, cancellationToken);

            return new Result(cart!.Id);
        }
    }
}
