using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Users;
using Portfolio.Domain.Users.Enums;
using SharedKernel;

namespace Portfolio.Application.Auth.RegisterEmail;

internal sealed class RegisterEmailCommandHandler(
    IApplicationDbContext db,
    IPasswordHasher hasher,
    IClaimsGenerator claimsGenerator,
    IRefreshTokenService rts,
    IConfiguration config)
    : ICommandHandler<RegisterEmailCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(RegisterEmailCommand cmd, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(cmd.FullName)) return Result.Failure<AuthResponse>(UserErrors.FullNameRequired);
        if (string.IsNullOrWhiteSpace(cmd.Email)) return Result.Failure<AuthResponse>(UserErrors.EmailRequired);
        if (string.IsNullOrWhiteSpace(cmd.Password)) return Result.Failure<AuthResponse>(UserErrors.PasswordRequired);

        var exists = await db.Users.AnyAsync(u => u.Email == cmd.Email, ct);
        if (exists) return Result.Failure<AuthResponse>(UserErrors.EmailInUse(cmd.Email));

        var user = new Domain.Users.User { FullName = cmd.FullName, Email = cmd.Email, PasswordHash = hasher.Hash(cmd.Password) };
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        var claims = await claimsGenerator.GenerateAsync(user.Id, "password", ct);
        var issued = await rts.IssueAsync(user.Id, claims, null, null, null, ct);

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
