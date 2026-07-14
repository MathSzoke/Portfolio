using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Users;
using Portfolio.Domain.Users.Enums;
using Shared.Api.Infrastructure.User;
using SharedKernel;

namespace Portfolio.Application.Auth.LoginGoogle;

internal sealed class LoginGoogleCommandHandler(
    IApplicationDbContext db,
    IHttpClientFactory httpFactory,
    IClaimsGenerator claimsGenerator,
    IRefreshTokenService rts,
    IConfiguration config)
    : ICommandHandler<LoginGoogleCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(LoginGoogleCommand cmd, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(cmd.AccessToken) && string.IsNullOrWhiteSpace(cmd.IdToken))
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var info = await GetGoogleUserAsync(cmd.AccessToken, ct);
        if (info is null || string.IsNullOrWhiteSpace(info.Email))
            return Result.Failure<AuthResponse>(UserErrors.ExternalLoginNotFound("Google", cmd.AccessToken!));

        var login = await db.ExternalLogins
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Provider == "google" && x.ProviderUserId == info.Sub, ct);

        if (login is null)
        {
            var existing = await db.Users.FirstOrDefaultAsync(u => u.Email == info.Email, ct);
            var user = existing ?? new Domain.Users.User
            {
                FullName = info.Name,
                Email = info.Email,
                ImageUrl = string.IsNullOrWhiteSpace(existing?.ImageUrl) ? NormalizeHttps(info.PictureUrl) : existing.ImageUrl
            };
            if (existing is null) db.Users.Add(user);
            login = new ExternalLogin
            {
                Provider = "google",
                ProviderUserId = info.Sub,
                User = user,
                UserPhotoUrl = info.PictureUrl
            };
            db.ExternalLogins.Add(login);
            await db.SaveChangesAsync(ct);
        }
        else
        {
            var updated = false;
            var pic = NormalizeHttps(info.PictureUrl);

            login.UserPhotoUrl = info.PictureUrl;

            if (!string.IsNullOrWhiteSpace(info.Name) && !string.Equals(login.User.FullName, info.Name, StringComparison.Ordinal))
            {
                login.User.FullName = info.Name;
                updated = true;
            }
            if (string.IsNullOrWhiteSpace(login.User.ImageUrl) && !string.IsNullOrWhiteSpace(pic))
            {
                login.User.ImageUrl = pic;
                updated = true;
            }
            if (updated)
                await db.SaveChangesAsync(ct);
        }

        var u = login.User;
        var claims = await claimsGenerator.GenerateAsync(u.Id, "google", ct);
        var issued = await rts.IssueAsync(u.Id, claims, null, null, new UserDeviceDetector(null).Find().Device, ct);

        var ttlMin = int.TryParse(config["Jwt:AccessTokenMinutes"], out var m) ? m : 15;

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

        async Task<GoogleUser?> GetGoogleUserAsync(string? accessToken, CancellationToken ct2)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) return null;
            var http = httpFactory.CreateClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var resp = await http.GetAsync("https://openidconnect.googleapis.com/v1/userinfo", ct2);
            if (resp.IsSuccessStatusCode)
            {
                var s = await resp.Content.ReadAsStringAsync(ct2);
                var v = JsonSerializer.Deserialize<UserInfoOidc>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return v is null ? null : new GoogleUser(v.Sub ?? string.Empty, v.Email ?? string.Empty, v.Name ?? string.Empty, v.Picture);
            }
            using var resp2 = await http.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo", ct2);
            if (!resp2.IsSuccessStatusCode) return null;
            var s2 = await resp2.Content.ReadAsStringAsync(ct2);
            var v2 = JsonSerializer.Deserialize<UserInfoV2>(s2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return v2 is null ? null : new GoogleUser(v2.Id ?? string.Empty, v2.Email ?? string.Empty, v2.Name ?? string.Empty, v2.Picture);
        }
    }

    static string? NormalizeHttps(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return url;
        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            return "https://" + url.AsSpan(7).ToString();
        return url;
    }

    private sealed record GoogleUser(string Sub, string Email, string Name, string? PictureUrl);
    private sealed record UserInfoOidc(string? Sub, string? Email, string? Name, string? Picture);
    private sealed record UserInfoV2(string? Id, string? Email, string? Name, string? Picture);
}
