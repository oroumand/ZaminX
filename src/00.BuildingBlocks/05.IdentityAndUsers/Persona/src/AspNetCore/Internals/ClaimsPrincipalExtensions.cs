using System.Security.Claims;

namespace ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.Internals;

internal static class ClaimsPrincipalExtensions
{
    public static string? GetClaimValue(this ClaimsPrincipal? principal, string claimType)
    {
        if (principal is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(claimType))
        {
            return null;
        }

        return principal.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
    }

    public static IReadOnlyCollection<string> GetClaimValues(this ClaimsPrincipal? principal, string claimType)
    {
        if (principal is null)
        {
            return Array.Empty<string>();
        }

        if (string.IsNullOrWhiteSpace(claimType))
        {
            return Array.Empty<string>();
        }

        return principal.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToArray();
    }
}