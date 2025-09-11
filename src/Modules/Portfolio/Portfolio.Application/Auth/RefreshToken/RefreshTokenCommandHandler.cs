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

        var issued = await rts.RefreshAsync(
            cmd.RefreshToken,
            uid => claimsGenerator.GenerateAsync(uid, "refresh", ct).GetAwaiter().GetResult(),
            null,
            null,
            new UserDeviceDetector(null).Find().Device,
            ct);

        return Result.Success(new RefreshResponse(
            issued.accessToken,
            ttlMin * 60,
            issued.refreshToken,
            issued.refreshExpires));
    }
}