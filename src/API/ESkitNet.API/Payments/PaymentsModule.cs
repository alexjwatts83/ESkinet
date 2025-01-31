using ESkitNet.Identity.Entities;

namespace ESkitNet.API.Payments;

public class PaymentsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/payments")
            .WithTags("Payments Module")
            //.RequireCors("CorsPolicy")
            ;

        group.MapPost("/{cartId}", CreateOrUpdateIntent.Endpoint.Handle)
            .WithName("CreateOrUpdateIntent")
            .RequireAuthorization();

        group.MapGet("/delivery-methods", GetMethods.Endpoint.Handle)
            .WithName("GetMethods");

        group.MapPost("/webhook", StripeWebhook.Endpoint.Handle)
            .WithName("StripeWebhook")
            .AllowAnonymous();

        group.MapIdentityApi<AppUser>();
    }
}
