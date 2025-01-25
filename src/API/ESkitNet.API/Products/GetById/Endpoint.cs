namespace ESkitNet.API.Products.GetById;

public record Response(ProductDto Product);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new Query(id));

            var response = result.Adapt<Response>();

            return Results.Ok(response);
        })
        .WithName("GetProduct")
        .Produces<Response>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product")
        .WithDescription("Get Product");
    }
}