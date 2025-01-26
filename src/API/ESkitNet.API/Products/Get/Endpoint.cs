namespace ESkitNet.API.Products.Get;

public record Response(PaginatedResult<ProductDto> Products);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] ProductsPaginationRequest request, ISender sender) =>
        {
            var result = await sender.Send(new Query(request));

            var response = result.Adapt<Response>();

            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<Response>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
