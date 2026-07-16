using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Auth;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Auth;
using Portfolio.Application.Auth.LoginGoogle;

namespace Portfolio.Api.Endpoints.Auth;

internal sealed class LoginGoogle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/google", async (
                [FromBody] GoogleLoginRequest request,
                [FromServices] ICommandHandler<LoginGoogleCommand, AuthResponse> handler,
                IConfiguration cfg,
                HttpContext http,
                CancellationToken ct) =>
            {
                var cmd = new LoginGoogleCommand(request.AccessToken, request.IdToken);
                var result = await handler.Handle(cmd, ct);

                return result.Match(
                    ok =>
                    {
                        RefreshTokenContract.SetRefreshCookie(http, cfg, ok.RefreshToken, ok.RefreshExpiresAtUtc);
                        return Results.Ok(ok);
                    },
                    CustomResults.Problem);
            })
            .WithTags(Tags.Auth);
    }
}