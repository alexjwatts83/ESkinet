namespace ESkitNet.API.Orders;

public class OrdersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/orders")
            .WithTags("Order Module")
            .RequireCors("CorsPolicy");

        //group.MapGet("/{id}", GetById.Endpoint.Handle)
        //    .WithName("GetOrder");

        group.MapPost("/", Create.Endpoint.Handle)
            .WithName("CreateOrder");

        //group.MapDelete("/{id}", Delete.Endpoint.Handle)
        //    .WithName("DeleteOrder");
    }
}
