using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Chat.Sessions.Status;

namespace Portfolio.Api.Endpoints.Chat.Sessions;

internal sealed class Status : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/chat/sessions/{sessionId:guid}/close", async (
                [FromRoute] Guid sessionId,
                [FromServices] ICommandHandler<CloseSessionCommand> closeHandler,
                CancellationToken ct) =>
            {
                var cmd = new CloseSessionCommand(sessionId);
                var result = await closeHandler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Chat)
            .RequireAuthorization("SuperAdminOnly");

        app.MapPut("api/v1/chat/sessions/{sessionId:guid}/archive", async (
                [FromRoute] Guid sessionId,
                [FromServices] ICommandHandler<ArchiveSessionCommand> archiveHandler,
                CancellationToken ct) =>
            {
                var cmd = new ArchiveSessionCommand(sessionId);
                var result = await archiveHandler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Chat)
            .RequireAuthorization("SuperAdminOnly");

        app.MapPut("api/v1/chat/sessions/{sessionId:guid}/reopen", async (
                [FromRoute] Guid sessionId,
                [FromServices] ICommandHandler<ReopenSessionCommand> reopenHandler,
                CancellationToken ct) =>
            {
                var cmd = new ReopenSessionCommand(sessionId);
                var result = await reopenHandler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Chat)
            .RequireAuthorization("SuperAdminOnly");
    }
}
