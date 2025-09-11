using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Portfolio.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var userIdValue = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
            throw new InvalidOperationException("User ID (sub) claim not found or invalid in token.");

        return userId;
    }
}