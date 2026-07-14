using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Abstractions.Security;
using Portfolio.Domain.Users;
using Portfolio.Domain.Users.Enums;
using Shared.Api.Infrastructure.User;
using SharedKernel;

namespace Portfolio.Application.Auth.LoginLinkedIn;

internal sealed class LoginLinkedInCommandHandler(
    IApplicationDbContext db,
    ILinkedInAuthService linkedIn,
    IClaimsGenerator claimsGenerator,
    IRefreshTokenService rts,
    IConfiguration config)
    : ICommandHandler<LoginLinkedInCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(LoginLinkedInCommand cmd, CancellationToken ct)
    {
        var accessToken = cmd.AccessToken;
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            var clientId = config["Auth:LinkedIn:ClientId"]!;
            var clientSecret = config["Auth:LinkedIn:ClientSecret"]!;
            try
            {
                accessToken = await linkedIn.ExchangeCodeForAccessToken(cmd.Code!, clientId, clientSecret, cmd.RedirectUri!, ct);
            }
            catch (Exception ex)
            {
                return Result.Failure<AuthResponse>(UserErrors.ExternalLoginNotFound("LinkedIn", ex.Message));
            }
        }

        var info = await linkedIn.GetUserInfo(accessToken, ct);

        var login = await db.ExternalLogins.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Provider == "linkedin" && x.ProviderUserId == info.Subject, ct);

        if (login is null)
        {
            var existing = await db.Users.FirstOrDefaultAsync(u => u.Email == info.Email, ct);
            var user = existing ?? new Domain.Users.User
            {
                FullName = info.Name,
                Email = info.Email,
                ImageUrl = string.IsNullOrWhiteSpace(existing?.ImageUrl) ? info.PictureUrl : existing.ImageUrl
            };
            if (existing is null) db.Users.Add(user);
            login = new ExternalLogin
            {
                Provider = "linkedin",
                ProviderUserId = info.Subject,
                User = user,
                UserPhotoUrl = info.PictureUrl
            };
            db.ExternalLogins.Add(login);
            await db.SaveChangesAsync(ct);
        }
        else
        {
            var updated = false;
            login.UserPhotoUrl = info.PictureUrl;

            if (!string.IsNullOrWhiteSpace(info.Name) && !string.Equals(login.User.FullName, info.Name, StringComparison.Ordinal))
            {
                login.User.FullName = info.Name;
                updated = true;
            }
            if (string.IsNullOrWhiteSpace(login.User.ImageUrl) && !string.IsNullOrWhiteSpace(info.PictureUrl))
            {
                login.User.ImageUrl = info.PictureUrl;
                updated = true;
            }
            if (updated)
                await db.SaveChangesAsync(ct);
        }

        var u = login.User;
        var claims = await claimsGenerator.GenerateAsync(u.Id, "linkedin", ct);
        var issued = await rts.IssueAsync(u.Id, claims, null, null, new UserDeviceDetector(null).Find().Device, ct);

        var ttlStr = config["Jwt:AccessTokenMinutes"];
        var ttlMin = int.TryParse(ttlStr, out var m) ? m : 15;

        var roles = u.Roles.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();

        u.Status = UserStatus.Available;
        
        await db.SaveChangesAsync(ct);

        return Result.Success(new AuthResponse(
            issued.accessToken,
            DateTime.UtcNow.AddMinutes(ttlMin),
            u.Id,
            u.Email!,
            u.FullName!,
            u.ImageUrl,
            roles,
            u.Status,
            issued.refreshToken,
            issued.refreshExpires,
            ttlMin));
    }
}
