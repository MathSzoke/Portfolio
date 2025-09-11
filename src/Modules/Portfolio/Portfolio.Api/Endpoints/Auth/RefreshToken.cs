using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Auth;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Auth.RefreshToken;

namespace Portfolio.Api.Endpoints.Auth;

internal sealed class RefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/refresh", async (
                [FromServices] ICommandHandler<RefreshTokenCommand, RefreshResponse> handler,
                IConfiguration cfg,
                HttpContext http,
                CancellationToken ct) =>
            {
                var cookieName = cfg["Jwt:CookieName"] ?? "rt";
                if (!http.Request.Cookies.TryGetValue(cookieName, out var rt) || string.IsNullOrWhiteSpace(rt))
                    return Results.Unauthorized();

                var result = await handler.Handle(new RefreshTokenCommand(rt), ct);

                RefreshTokenContract.SetRefreshCookie(http, cfg, result.Value.RefreshToken, result.Value.RefreshExpiresAtUtc);
                return Results.Ok(new { accessToken = result.Value.AccessToken, expiresInSeconds = result.Value.ExpiresInSeconds });
            })
            .WithTags(Tags.Auth);
    }
}