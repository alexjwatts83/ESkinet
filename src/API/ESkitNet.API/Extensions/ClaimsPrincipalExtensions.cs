using ESkitNet.API.Accounts.Dtos;
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

    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal, bool includAddress = false)
    {
        var allUsers = userManager.Users;

        if (includAddress)
            allUsers = allUsers.Include(x => x.Address);

        var user = await allUsers
            .FirstOrDefaultAsync(x => x.Email == claimsPrincipal.GetEmail());

        return user ?? throw new AuthenticationException("User not found");
    }
}
public static class AddressExtensions
{
    public static void UpdateFromDto(this Address address, AddressDto dto)
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(dto);

        address.Line1 = dto.Line1;
        address.Line2 = dto.Line2;
        address.City = dto.City;
        address.State = dto.State;
        address.PostalCode = dto.PostalCode;
        address.Country = dto.Country;
    }
}
