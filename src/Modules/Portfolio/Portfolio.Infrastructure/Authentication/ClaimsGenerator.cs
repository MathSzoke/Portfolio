using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;

namespace Portfolio.Infrastructure.Authentication;

internal sealed class ClaimsGenerator(IApplicationDbContext context) : IClaimsGenerator
{
    public async Task<List<Claim>> GenerateAsync(Guid userId, string idp, CancellationToken ct = default)
    {
        var u = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, ct) ?? throw new InvalidOperationException("User not found");

        var now = DateTimeOffset.UtcNow;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, u.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, u.Email!),
            new("name", u.FullName!),
            new("idp", idp),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        if (!string.IsNullOrWhiteSpace(u.ImageUrl))
            claims.Add(new Claim("picture", u.ImageUrl));

        claims.AddRange(u.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        return claims;
    }
}
