using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Users;
using Portfolio.Domain.Users.Enums;
using Shared.Api.Infrastructure.User;
using SharedKernel;

namespace Portfolio.Application.Auth.LoginEmail;

internal sealed class LoginEmailCommandHandler(
    IApplicationDbContext db,
    IPasswordHasher hasher,
    IClaimsGenerator claimsGenerator,
    IRefreshTokenService rts,
    IConfiguration config)
    : ICommandHandler<LoginEmailCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(LoginEmailCommand cmd, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(cmd.Email) || string.IsNullOrWhiteSpace(cmd.Password))
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == cmd.Email, ct);
        if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash) || !hasher.Verify(cmd.Password, user.PasswordHash))
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var claims = await claimsGenerator.GenerateAsync(user.Id, "password", ct);
        var issued = await rts.IssueAsync(user.Id, claims, null, null, new UserDeviceDetector(null).Find().Device, ct);

        var ttlStr = config["Jwt:AccessTokenMinutes"];
        var ttlMin = int.TryParse(ttlStr, out var m) ? m : 15;

        var roles = (user.Roles)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim())
            .ToArray();

        user.Status = UserStatus.Available;
        
        await db.SaveChangesAsync(ct);

        return Result.Success(new AuthResponse(
            issued.accessToken,
            DateTime.UtcNow.AddMinutes(ttlMin),
            user.Id,
            user.Email!,
            user.FullName!,
            user.ImageUrl,
            roles,
            user.Status,
            issued.refreshToken,
            issued.refreshExpires,
            ttlMin));
    }
}