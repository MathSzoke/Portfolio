using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Chat.Sessions.HasOpen;
using System.Security.Claims;

namespace Portfolio.Api.Endpoints.Chat.Sessions;

internal sealed class HasOpen : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/chat/sessions/me/has-open", [AllowAnonymous] async (
                ClaimsPrincipal user,
                [FromServices] IQueryHandler<HasOpenSessionQuery, bool> handler,
                CancellationToken ct) =>
        {
            var email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var query = new HasOpenSessionQuery(email);
            var result = await handler.Handle(query, ct);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
            .WithTags(Tags.Chat);
    }
}
