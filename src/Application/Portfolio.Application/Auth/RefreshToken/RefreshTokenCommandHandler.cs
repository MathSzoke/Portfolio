using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Messaging;
using Shared.Api.Infrastructure.User;
using SharedKernel;

namespace Portfolio.Application.Auth.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IRefreshTokenService rts,
    IClaimsGenerator claimsGenerator,
    IConfiguration config)
    : ICommandHandler<RefreshTokenCommand, RefreshResponse>
{
    public async Task<Result<RefreshResponse>> Handle(RefreshTokenCommand cmd, CancellationToken ct)
    {
        var ttlMin = int.TryParse(config["Jwt:AccessTokenMinutes"], out var m) ? m : 15;

        var result = await rts.RefreshAsync(
            cmd.RefreshToken,
            uid => claimsGenerator.GenerateAsync(uid, "refresh", ct).GetAwaiter().GetResult(),
            null,
            null,
            new UserDeviceDetector(null).Find().Device,
            ct);

        if (result.IsFailure)
        {
            return Result.Failure<RefreshResponse>(result.Error);
        }

        return Result.Success(new RefreshResponse(
            result.Value.accessToken,
            ttlMin * 60,
            result.Value.refreshToken,
            result.Value.refreshExpires));
    }
}