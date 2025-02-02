using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ESkitNet.API.Buggy;

public class Module : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/buggy/unauthorised", () =>
        {
            return Results.Unauthorized();
        })
        .WithName("BuggyUnauthorised")
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("BuggyUnauthorised")
        .WithDescription("Buggy Unauthorised");

        app.MapGet("api/buggy/not-found", () =>
        {
            return Results.NotFound();
        })
        .WithName("BuggyNotFound")
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Buggy NotFound")
        .WithDescription("Buggy NotFound");

        app.MapGet("api/buggy/bad-request", () =>
        {
            return Results.BadRequest();
        })
        .WithName("BuggyBadRequest")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Buggy BadRequest")
        .WithDescription("Buggy BadRequest");

        app.MapGet("api/buggy/internal-error", () =>
        {
            throw new Exception("Tis an intention error");
        })
        .WithName("BuggyInternalError")
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Buggy InternalError")
        .WithDescription("Buggy InternalError");

        app.MapPost("api/buggy/validation-error", (Product product) =>
        {
            return Results.Ok();
        })
        .WithName("BuggyValidationError")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Buggy ValidationError")
        .WithDescription("Buggy ValidationError");

        app.MapGet("api/buggy/secret", (HttpContext context) =>
        {
            var name = context.User.FindFirstValue(ClaimTypes.Name);
            var email = context.User.FindFirstValue(ClaimTypes.Email);
            var id = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var message = $"Name: {name}, Email: {email}, Id: {id}";

            return Results.Ok(message);
        })
        .WithName("BuggySecret")
        .RequireAuthorization()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Buggy Secret")
        .WithDescription("Buggy Secret");

        app.MapGet("api/buggy/admin-secret", [Authorize(Roles = "Admin")] (HttpContext context) =>
        {
            return Results.Ok(new {
                Name = context.User.FindFirstValue(ClaimTypes.Name),
                Email = context.User.FindFirstValue(ClaimTypes.Email),
                Id = context.User.FindFirstValue(ClaimTypes.NameIdentifier),
                Roles = context.User.FindFirstValue(ClaimTypes.Role)
            });
        })
        .WithName("BuggyAdmiSecret")
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Buggy Admin Secret")
        .WithDescription("Buggy Admin Secret");
    }
}