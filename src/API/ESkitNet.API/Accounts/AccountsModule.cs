using ESkitNet.Identity.Entities;

namespace ESkitNet.API.Accounts;

public class AccountsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/accounts")
            .WithTags("Accounts Module");
            //.MapIdentityApi<AppUser>();

        // TODO: figure out a way to use just `/register` and disable current register
        group.MapPost("/custom-register", Register.Endpoint.Handle)
            .WithName("RegisterUser")
            .Produces<Register.Endpoint.Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Register User")
            .WithDescription("Register User");

        group.MapPost("/logout", Logout.Endpoint.Handle)
            .WithName("LogoutUser")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Logout User")
            .WithDescription("Logout User");

        group.MapGet("/user-info", UserInfo.Endpoint.Handle)
            .WithName("UserInfo");

        group.MapGet("/auth-status", AuthCheck.Endpoint.Handle)
            .WithName("AuthCheck");

        // TODO there really should a 2 different endpoints to create and update address
        group.MapPost("/address", AddOrUpdateAddress.Endpoint.Handle)
            .WithName("AddOrUpdateAddress")
            .RequireAuthorization();

        group.MapIdentityApi<AppUser>();
    }
}
