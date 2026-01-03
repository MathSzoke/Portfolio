using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Domain.Auth;
using SharedKernel;

namespace Portfolio.Infrastructure.Authentication;

internal sealed class RefreshTokenService(
    IApplicationDbContext db,
    ITokenProvider accessTokens,
    IConfiguration cfg)
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
        var sliding = TimeSpan.FromMinutes(cfg.GetValue<int>("Jwt:RefreshSlidingMinutes"));
        var absolute = TimeSpan.FromDays(cfg.GetValue<int>("Jwt:RefreshAbsoluteDays"));

        var token = GenerateToken();
        var hash = Hash(token);

        var rt = new RefreshToken
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
        await db.SaveChangesAsync(ct);

        var at = accessTokens.Create(claims);
        return (at, token, rt.ExpiresAtUtc);
    }

    public async Task<Result<(string accessToken, string refreshToken, DateTime refreshExpires)>> RefreshAsync(
        string refreshToken,
        Func<Guid, IEnumerable<Claim>> claimsFactory,
        string? ip,
        string? ua,
        string? device,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Result.Failure<(string, string, DateTime)>(RefreshTokenErrors.InvalidRefreshToken);
        }

        var now = DateTime.UtcNow;
        var sliding = TimeSpan.FromMinutes(cfg.GetValue<int>("Jwt:RefreshSlidingMinutes"));
        var hash = Hash(refreshToken);

        var current = await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == hash && x.RevokedAtUtc == null, ct);
        
        if (current is null)
        {
            return Result.Failure<(string, string, DateTime)>(RefreshTokenErrors.TokenNotFound);
        }
        
        if (now > current.MaxExpiresAtUtc)
        {
            return Result.Failure<(string, string, DateTime)>(RefreshTokenErrors.LifetimeReached);
        }

        var nextToken = GenerateToken();
        var nextHash = Hash(nextToken);

        var nextExpires = now.Add(sliding);
        if (nextExpires > current.MaxExpiresAtUtc)
            nextExpires = current.MaxExpiresAtUtc;

        current.TokenHash = nextHash;
        current.CreatedAtUtc = now;
        current.ExpiresAtUtc = nextExpires;
        current.IpAddress = ip;
        current.UserAgent = ua;
        if (!string.IsNullOrWhiteSpace(device)) current.Device = device;

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            return Result.Failure<(string, string, DateTime)>(
                RefreshTokenErrors.ErrorInternal);
        }

        var claims = claimsFactory(current.UserId);
        var at = accessTokens.Create(claims);

        return Result.Success((at, nextToken, current.ExpiresAtUtc));
    }

    public async Task RevokeAsync(string refreshToken, string reason, CancellationToken ct)
    {
        var hash = Hash(refreshToken);
        var current = await db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);
        if (current is null) return;

        current.RevokedAtUtc = DateTime.UtcNow;
        current.RevokedReason = reason;
        await db.SaveChangesAsync(ct);
    }

    static string GenerateToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

    static string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}