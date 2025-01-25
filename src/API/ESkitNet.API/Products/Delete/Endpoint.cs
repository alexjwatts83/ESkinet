namespace ESkitNet.API.Products.Delete;

public record Response(bool IsSuccess);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new Command(id));

            var response = result.Adapt<Response>();

            // TODO return better response
            return (response == null)
             ? Results.BadRequest("Failed to delete Product")
             : Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces<Response>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");
    }
}
