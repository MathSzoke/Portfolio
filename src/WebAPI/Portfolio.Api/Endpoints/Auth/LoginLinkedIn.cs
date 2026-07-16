using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Auth;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Auth;
using Portfolio.Application.Auth.LoginLinkedIn;

namespace Portfolio.Api.Endpoints.Auth;

internal sealed class LoginLinkedIn : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/linkedin", async (
                [FromBody] LinkedInLoginRequest request,
                [FromServices] ICommandHandler<LoginLinkedInCommand, AuthResponse> handler,
                IConfiguration cfg,
                HttpContext http,
                CancellationToken ct) =>
            {
                var cmd = new LoginLinkedInCommand(request.Code, request.AccessToken, request.RedirectUri);
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