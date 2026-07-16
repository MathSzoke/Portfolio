using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Chat.Messages.List;

namespace Portfolio.Api.Endpoints.Chat.Messages;

internal sealed class List : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/chat/sessions/{sessionId:guid}/messages", async (
                [FromRoute] Guid sessionId,
                [FromQuery] DateTimeOffset? after,
                [FromServices] IQueryHandler<ListMessagesQuery, ChatMessagesResponse> handler,
                CancellationToken ct) =>
            {
                var query = new ListMessagesQuery(sessionId, after);
                var result = await handler.Handle(query, ct);
                return result.Match(
                    dto => Results.Ok(dto),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Chat);
    }
}
