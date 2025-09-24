using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Auth;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Auth.RefreshToken;

namespace Portfolio.Api.Endpoints.Auth;

internal sealed class RefreshToken : IEndpoint
{
    public sealed record RefreshRequest(string RefreshTokenStorage);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/refresh", async (
                [FromBody] RefreshRequest request,
                [FromServices] ICommandHandler<RefreshTokenCommand, RefreshResponse> handler,
                IConfiguration cfg,
                HttpContext http,
                CancellationToken ct) =>
            {
                var result = await handler.Handle(new RefreshTokenCommand(request.RefreshTokenStorage), ct);

                RefreshTokenContract.SetRefreshCookie(http, cfg, result.Value.RefreshToken, result.Value.RefreshExpiresAtUtc);
                return Results.Ok(new { accessToken = result.Value.AccessToken, expiresInSeconds = result.Value.ExpiresInSeconds, refreshToken = result.Value.RefreshToken });
            })
            .WithTags(Tags.Auth);
    }
}