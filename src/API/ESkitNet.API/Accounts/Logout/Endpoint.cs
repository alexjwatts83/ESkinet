using ESkitNet.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace ESkitNet.API.Accounts.Logout;

public static class Endpoint
{
    public static async Task<IResult> Handle(IServiceProvider sp)
    {
        var signInManager = sp.GetRequiredService<SignInManager<AppUser>>();

        await signInManager.SignOutAsync();

        return Results.NoContent();
    }
}
