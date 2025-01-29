namespace ESkitNet.API.Payments.CreateOrUpdateIntent;

public static class Endpoint
{
    public record Response(ShoppingCart Cart);

    public static async Task<IResult> Handle(string cartId, ISender sender)
    {
        var result = await sender.Send(new Command(cartId));

        var response = result.Adapt<Response>();

        return Results.Ok(response.Cart);
    }

    public record Command(string CartId) : ICommand<Result>;
    public record Result(ShoppingCart Cart);

    public class Handler(IPaymentService paymentService, IGenericRepository<DeliveryMethod, DeliveryMethodId> repo) 
        : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(command.CartId, cancellationToken);

            if (cart == null)
                throw new BadHttpRequestException("There was a problem processing your cart", StatusCodes.Status400BadRequest);

            return new Result(cart);
        }
    }
}
