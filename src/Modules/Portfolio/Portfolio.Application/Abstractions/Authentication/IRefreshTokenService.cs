using System.Security.Claims;
using SharedKernel;

namespace Portfolio.Application.Abstractions.Authentication;

public interface IRefreshTokenService
{
    Task<(string accessToken, string refreshToken, DateTime refreshExpires)> IssueAsync(
        Guid userId,
        IEnumerable<Claim> claims,
        string? ip,
        string? ua,
        string? device,
        CancellationToken ct);

    Task<Result<(string accessToken, string refreshToken, DateTime refreshExpires)>> RefreshAsync(
        string refreshToken,
        Func<Guid, IEnumerable<Claim>> claimsFactory,
        string? ip,
        string? ua,
        string? device,
        CancellationToken ct);

    Task RevokeAsync(string refreshToken, string reason, CancellationToken ct);
}