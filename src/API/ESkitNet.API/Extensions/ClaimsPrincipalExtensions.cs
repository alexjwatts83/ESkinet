using ESkitNet.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Security.Claims;

namespace ESkitNet.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        return email ?? throw new AuthenticationException("Email Claim not found");
    }

    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(x => x.Email == claimsPrincipal.GetEmail());

        return user ?? throw new AuthenticationException("User not found");
    }
}