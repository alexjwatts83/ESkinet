using System.Security.Authentication;
using System.Security.Claims;

namespace ESkitNet.Core.Extensions;
public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal, bool throwException)
    {
        //var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        //return email ?? throw new AuthenticationException("Email Claim not found");

        return GetValue(claimsPrincipal, ClaimTypes.Email, throwException);
    }

    //public static string? GetNameIdentifier(this ClaimsPrincipal claimsPrincipal, bool throwException)
    //{
    //    var nameIdentifier = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

    //    if (nameIdentifier == null && throwException)
    //        throw new AuthenticationException("Name Identifier not found");

    //    return nameIdentifier ?? string.Empty;
    //}

    private static string? GetValue(ClaimsPrincipal claimsPrincipal, string indentifier, bool throwException)
    {
        var nameIdentifier = claimsPrincipal.FindFirstValue(indentifier);

        if (nameIdentifier == null && throwException)
            throw new AuthenticationException("Name Identifier not found");

        return nameIdentifier;
    }
}
