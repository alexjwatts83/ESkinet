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
            .WithName("GetForUser");

        group.MapPost("/", Create.Endpoint.Handle)
            .WithName("CreateOrder");

        group.MapGet("/{id}", GetForUserById.Endpoint.Handle)
            .WithName("GetForUserById");
    }
}
