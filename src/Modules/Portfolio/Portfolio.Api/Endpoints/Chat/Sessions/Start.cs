using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Chat;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Chat.Sessions.Start;

namespace Portfolio.Api.Endpoints.Chat.Sessions;

internal sealed class Start : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/chat/sessions", async (
                [FromBody] StartSessionRequest request,
                [FromServices] ICommandHandler<StartSessionCommand, ChatSessionResponse> handler,
                CancellationToken ct) =>
            {
                var cmd = new StartSessionCommand(request.Name, request.Email);
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Chat);
    }
}
