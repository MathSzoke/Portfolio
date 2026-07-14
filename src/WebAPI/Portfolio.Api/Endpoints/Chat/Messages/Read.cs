using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Chat.Messages.Read;

namespace Portfolio.Api.Endpoints.Chat.Messages;

internal sealed class Read : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/chat/messages/{messageId:guid}/read", async (
                [FromRoute] Guid messageId,
                [FromServices] ICommandHandler<MarkMessageReadCommand> handler,
                CancellationToken ct) =>
            {
                var cmd = new MarkMessageReadCommand(messageId);
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Chat)
            .RequireAuthorization("SuperAdminOnly");
    }
}
