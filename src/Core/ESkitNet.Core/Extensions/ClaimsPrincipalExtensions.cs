using System.Security.Authentication;
using System.Security.Claims;

namespace ESkitNet.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal, bool throwException)
    {
        return GetValue(claimsPrincipal, ClaimTypes.Email, throwException);
    }

    private static string? GetValue(ClaimsPrincipal claimsPrincipal, string indentifier, bool throwException)
    {
        var nameIdentifier = claimsPrincipal.FindFirstValue(indentifier);

        if (nameIdentifier == null && throwException)
            throw new AuthenticationException($"{indentifier} Identifier not found");

        return nameIdentifier;
    }
}
