namespace ESkitNet.API.Types.Get;

public record Response(IReadOnlyList<string> Types);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/types", async (ISender sender, string sort = "") =>
        {
            var result = await sender.Send(new Query(sort));

            var response = result.Adapt<Response>();

            return Results.Ok(response.Types);
        })
        .WithName("GetTypes")
        .Produces<IReadOnlyList<string>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Types")
        .WithDescription("Get Types");
    }
}
