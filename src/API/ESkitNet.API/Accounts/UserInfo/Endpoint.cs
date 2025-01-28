using ESkitNet.API.Extensions;
using ESkitNet.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace ESkitNet.API.Accounts.UserInfo;

public static class Endpoint
{
    public static async Task<IResult> Handle(IServiceProvider sp, HttpContext context)
    {
        var claimsPrincipal = context.User;
        if (claimsPrincipal.Identity?.IsAuthenticated == false)
            return Results.NoContent();

        var signInManager = sp.GetRequiredService<SignInManager<AppUser>>();

        var user = await signInManager.UserManager.GetUserByEmail(claimsPrincipal, true);

        return Results.Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.Address
        });
    }
}
