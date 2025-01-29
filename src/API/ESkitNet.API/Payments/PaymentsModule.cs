using ESkitNet.Identity.Entities;

namespace ESkitNet.API.Payments;

public class PaymentsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/payments")
            .WithTags("Payments Module");
        //.MapIdentityApi<AppUser>();

        group.MapPost("/{cartId}", CreateOrUpdateIntent.Endpoint.Handle)
            .WithName("CreateOrUpdateIntent")
            .RequireAuthorization();

        group.MapGet("/delivery-methods", GetMethods.Endpoint.Handle)
            .WithName("GetMethods");

        group.MapIdentityApi<AppUser>();
    }
}
