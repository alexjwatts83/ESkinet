using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ESkitNet.API.Buggy;

public class Module : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/buggy/unauthorised", () =>
        {
            return Results.Unauthorized();
        })
        .WithName("BuggyUnauthorised")
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("BuggyUnauthorised")
        .WithDescription("Buggy Unauthorised");

        app.MapGet("/buggy/not-found", () =>
        {
            return Results.NotFound();
        })
        .WithName("BuggyNotFound")
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Buggy NotFound")
        .WithDescription("Buggy NotFound");

        app.MapGet("/buggy/bad-request", () =>
        {
            return Results.BadRequest();
        })
        .WithName("BuggyBadRequest")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Buggy BadRequest")
        .WithDescription("Buggy BadRequest");

        app.MapGet("/buggy/internal-error", () =>
        {
            throw new Exception("Tis an intention error");
        })
        .WithName("BuggyInternalError")
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Buggy InternalError")
        .WithDescription("Buggy InternalError");

        app.MapPost("/buggy/validation-error", (Product product) =>
        {
            return Results.Ok();
        })
        .WithName("BuggyValidationError")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Buggy ValidationError")
        .WithDescription("Buggy ValidationError");

        app.MapGet("/buggy/secret", (HttpContext context) =>
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
    }
}