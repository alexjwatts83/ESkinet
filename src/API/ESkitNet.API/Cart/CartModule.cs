namespace ESkitNet.API.Cart;

public class CartModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/cart")
            .WithTags("Cart Module")
            .RequireCors("CorsPolicy");

        group.MapGet("/{id}", GetById.Endpoint.Handle)
            .WithName("GetCart")
            .Produces<ShoppingCart>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Cart")
            .WithDescription("Get Cart");

        group.MapPost("/", Create.Endpoint.Handle)
            .WithName("CreateCart")
            .Produces<Create.Endpoint.Response>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Cart")
            .WithDescription("Create Cart");

        group.MapDelete("/{id}", Delete.Endpoint.Handle)
            .WithName("DeleteCart")
            .Produces<Delete.Endpoint.Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Cart")
            .WithDescription("Delete Cart");
    }
}
