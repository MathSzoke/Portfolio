using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Chat;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Chat.Messages.Post;

namespace Portfolio.Api.Endpoints.Chat.Messages;

internal sealed class Post : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/chat/sessions/{sessionId:guid}/messages", async (
                [FromRoute] Guid sessionId,
                [FromBody] PostMessageRequest request,
                [FromServices] ICommandHandler<PostMessageCommand, ChatMessageResponse> handler,
                CancellationToken ct) =>
        {
            var cmd = new PostMessageCommand(sessionId, request.Content, request.AsMe);
            var result = await handler.Handle(cmd, ct);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
            .WithTags(Tags.Chat);
    }
}
