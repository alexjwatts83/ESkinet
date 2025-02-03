namespace ESkitNet.API.Admin;

public class AdminModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/admin")
            .WithTags("Admin Module")
            .RequireCors("CorsPolicy")
            .RequireAuthorization(x => x.RequireRole("Admin"));

        group.MapGet("/orders", GetOrders.Endpoint.Handle)
            .WithName("AdminGetOrders");

        group.MapGet("/orders/{id}", GetOrderById.Endpoint.Handle)
            .WithName("AdminGetOrderById");

        group.MapPost("/orders/{id}/refund", RefundOrder.Endpoint.Handle)
            .WithName("AdminRefundOrder");
    }
}
