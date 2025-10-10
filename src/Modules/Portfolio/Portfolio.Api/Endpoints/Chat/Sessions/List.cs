using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Chat.Sessions.List;

namespace Portfolio.Api.Endpoints.Chat.Sessions;

internal sealed class List : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/chat/sessions", async (
                [FromQuery] Guid? UserId,
                [FromServices] IQueryHandler<ListSessionsQuery, ChatSessionsResponse> handler,
                CancellationToken ct) =>
        {
            var query = new ListSessionsQuery(UserId);
            var result = await handler.Handle(query, ct);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Chat)
        .RequireAuthorization();
    }
}