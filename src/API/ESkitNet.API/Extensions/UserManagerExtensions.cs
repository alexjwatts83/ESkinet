using ESkitNet.Core.Extensions;
using ESkitNet.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Security.Claims;

namespace ESkitNet.API.Extensions;

public static class UserManagerExtensions
{
    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal, bool includAddress = false)
    {
        var allUsers = userManager.Users;

        if (includAddress)
            allUsers = allUsers.Include(x => x.Address);

        var user = await allUsers
            .FirstOrDefaultAsync(x => x.Email == claimsPrincipal.GetEmail(true));

        return user ?? throw new AuthenticationException("User not found");
    }
}
