namespace ESkitNet.API.Orders;

public class OrdersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/orders")
            .WithTags("Order Module")
            .RequireCors("CorsPolicy");

        group.MapGet("/", GetForUser.Endpoint.Handle)
            .WithName("GetOrdersForUser");

        group.MapPost("/", CreateOrUpdate.Endpoint.Handle)
            .WithName("CreateOrUpdateOrder");

        group.MapGet("/{id}", GetForUserById.Endpoint.Handle)
            .WithName("GetOrderForUserById");
    }
}
