using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Domain.Auth;

namespace Portfolio.Infrastructure.Authentication;

internal sealed class RefreshTokenService(IApplicationDbContext db, ITokenProvider accessTokens, IConfiguration cfg)
    : IRefreshTokenService
{
    public async Task<(string accessToken, string refreshToken, DateTime refreshExpires)> IssueAsync(
        Guid userId,
        IEnumerable<Claim> claims,
        string? ip,
        string? ua,
        string? device,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var sliding = TimeSpan.FromMinutes(cfg.GetValue<int?>("Jwt:RefreshSlidingMinutes") ?? 10080);
        var absolute = TimeSpan.FromDays(cfg.GetValue<int?>("Jwt:RefreshAbsoluteDays") ?? 30);

        var token = GenerateToken();
        var hash = Hash(token);

        RefreshToken? rt = null;
        if (!string.IsNullOrWhiteSpace(device))
        {
            rt = await db.RefreshTokens
                .Where(x => x.UserId == userId && x.Device == device && x.RevokedAtUtc == null && now <= x.MaxExpiresAtUtc)
                .OrderByDescending(x => x.CreatedAtUtc)
                .FirstOrDefaultAsync(ct);
        }

        if (rt is null)
        {
            rt = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TokenHash = hash,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.Add(sliding),
                MaxExpiresAtUtc = now.Add(absolute),
                IpAddress = ip,
                UserAgent = ua,
                Device = device
            };
            db.RefreshTokens.Add(rt);
        }
        else
        {
            rt.TokenHash = hash;
            rt.CreatedAtUtc = now;
            rt.ExpiresAtUtc = now.Add(sliding);
            rt.MaxExpiresAtUtc = now.Add(absolute);
            rt.IpAddress = ip;
            rt.UserAgent = ua;
        }

        await db.SaveChangesAsync(ct);

        var at = accessTokens.Create(claims);
        return (at, token, rt.ExpiresAtUtc);
    }

    public async Task<(string accessToken, string refreshToken, DateTime refreshExpires)> RefreshAsync(
        string refreshToken,
        Func<Guid, IEnumerable<Claim>> claimsFactory,
        string? ip,
        string? ua,
        string? device,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var sliding = TimeSpan.FromMinutes(cfg.GetValue<int?>("Jwt:RefreshSlidingMinutes") ?? 10080);

        var hash = Hash(refreshToken);
        var current = await db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);
        if (current is null) throw new InvalidOperationException("invalid_refresh_token");
        if (current.RevokedAtUtc is not null) throw new InvalidOperationException("revoked_refresh_token");
        if (now > current.ExpiresAtUtc) throw new InvalidOperationException("expired_refresh_token");
        if (now > current.MaxExpiresAtUtc) throw new InvalidOperationException("max_lifetime_reached");

        var nextToken = GenerateToken();
        var nextHash = Hash(nextToken);

        var nextExp = now.Add(sliding);
        if (nextExp > current.MaxExpiresAtUtc) nextExp = current.MaxExpiresAtUtc;

        current.TokenHash = nextHash;
        current.ExpiresAtUtc = nextExp;
        current.IpAddress = ip;
        current.UserAgent = ua;
        if (!string.IsNullOrWhiteSpace(device)) current.Device = device;

        await db.SaveChangesAsync(ct);

        var claims = claimsFactory(current.UserId);
        var at = accessTokens.Create(claims);
        return (at, nextToken, current.ExpiresAtUtc);
    }

    public async Task RevokeAsync(string refreshToken, string reason, CancellationToken ct)
    {
        var hash = Hash(refreshToken);
        var current = await db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);
        if (current is null) return;
        if (current.RevokedAtUtc is not null) return;
        current.RevokedAtUtc = DateTime.UtcNow;
        current.RevokedReason = reason;
        await db.SaveChangesAsync(ct);
    }

    static string GenerateToken()
    {
        Span<byte> bytes = stackalloc byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Base64UrlEncode(bytes);
    }

    static string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }

    static string Base64UrlEncode(ReadOnlySpan<byte> bytes)
    {
        var s = Convert.ToBase64String(bytes);
        return s.Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
