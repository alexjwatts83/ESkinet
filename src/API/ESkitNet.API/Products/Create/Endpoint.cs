namespace ESkitNet.API.Products.Create;

public record Request(ProductDto Product);
public record Response(Guid Id);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (Request request, ISender sender) =>
        {
            var command = request.Adapt<Command>();

            var result = await sender.Send(command);

            var response = result.Adapt<Response>();

            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<Response>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}
