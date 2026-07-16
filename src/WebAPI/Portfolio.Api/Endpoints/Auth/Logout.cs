using Portfolio.Api.Contracts.Auth;
using Portfolio.Application.Abstractions.Authentication;

namespace Portfolio.Api.Endpoints.Auth;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/logout", async (
                IConfiguration cfg,
                HttpContext http,
                IRefreshTokenService rts,
                CancellationToken ct) =>
            {
                var name = cfg["Jwt:CookieName"] ?? "__Host-rt";
                if (http.Request.Cookies.TryGetValue(name, out var rt) && !string.IsNullOrWhiteSpace(rt))
                    await rts.RevokeAsync(rt, "user_logout", ct);

                RefreshTokenContract.ClearRefreshCookie(http, cfg);
                return Results.NoContent();
            })
            .WithTags(Tags.Auth);
    }
}