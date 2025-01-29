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
            RuleForEach(x => x.Cart.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotNull().NotEmpty();
                item.RuleFor(i => i.ProductName).NotNull().NotEmpty();
                item.RuleFor(i => i.PictureUrl).NotNull().NotEmpty();
                item.RuleFor(i => i.Type).NotNull().NotEmpty();
                item.RuleFor(i => i.Brand).NotNull().NotEmpty();
                item.RuleFor(i => i.Price).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
                item.RuleFor(i => i.Quantity).NotNull().NotEmpty().GreaterThanOrEqualTo(1);
            });
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
