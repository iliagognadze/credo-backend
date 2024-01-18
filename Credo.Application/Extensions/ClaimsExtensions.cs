using System.Security.Claims;

namespace Credo.Application.Extensions;

public static class ClaimsExtensions
{
    public static string GetClaimValue(this IEnumerable<Claim> claims, string claimType) =>
        claims.FirstOrDefault(claim => claim.Type == claimType)?.Value
        ?? throw new Exception($"Claim with type {claimType} could not be found in token.");
}