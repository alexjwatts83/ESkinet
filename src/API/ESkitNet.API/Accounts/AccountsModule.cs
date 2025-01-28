using ESkitNet.Identity.Entities;

namespace ESkitNet.API.Accounts;

public class AccountsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/accounts")
            .WithTags("Accounts Module");
            //.MapIdentityApi<AppUser>();

        // TODO: figure out a way to use just `/register` and disable current register
        group.MapPost("/custom-register", Register.Endpoint.Handle)
            .WithName("RegisterUser")
            .Produces<Register.Endpoint.Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Register User")
            .WithDescription("Register User");

        group.MapIdentityApi<AppUser>();
    }
}
