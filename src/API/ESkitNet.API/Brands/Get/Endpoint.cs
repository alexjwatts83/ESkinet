namespace ESkitNet.API.Brands.Get;

public record Response(IReadOnlyList<string> Brands);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands", async (ISender sender) =>
        {
            var result = await sender.Send(new Query());

            var response = result.Adapt<Response>();

            return Results.Ok(response);
        })
        .WithName("GetBrands")
        .Produces<Response>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
