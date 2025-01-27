namespace ESkitNet.API.Cart.GetById;

public static class Endpoint
{
    public record Response(ShoppingCart Cart);
    public record Query(string Id) : IQuery<Result>;
    public record Result(ShoppingCart Cart);

    public class Handler(IShoppingCartService cartService) : IQueryHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetAsync(query.Id, cancellationToken);

            if (cart is null)
                throw new CartNotFoundException(query.Id);

            return new Result(cart);
        }
    }

    public static async Task<IResult> Handle(string id, ISender sender)
    {
        var result = await sender.Send(new Query(id));

        var response = result.Adapt<Response>();

        return Results.Ok(response.Cart);
    }
}