namespace ESkitNet.API.Accounts.AuthCheck;

public static class Endpoint
{
    public static IResult Handle(HttpContext context)
    {
        var contextUser = context.User;

        return Results.Ok(new
        {
            IsAuthenticated = contextUser.Identity?.IsAuthenticated ?? false
        });
    }
}
