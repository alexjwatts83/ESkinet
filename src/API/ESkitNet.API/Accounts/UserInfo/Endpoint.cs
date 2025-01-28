using ESkitNet.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ESkitNet.API.Accounts.UserInfo;

public static class Endpoint
{
    public static async Task<IResult> Handle(IServiceProvider sp, HttpContext context)
    {
        var contextUser = context.User;
        if (contextUser.Identity?.IsAuthenticated == false)
            return Results.NoContent();

        var signInManager = sp.GetRequiredService<SignInManager<AppUser>>();

        var user = await signInManager.UserManager.Users
            .FirstOrDefaultAsync(x => x.Email == contextUser.FindFirstValue(ClaimTypes.Email));

        // not really sure this will happen if the user is already authenticated, maybe they were deleted???
        if (user == null) 
            return Results.Unauthorized();

        return Results.Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email
        });
    }
}
