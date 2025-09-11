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
                [FromQuery] string? status,
                [FromQuery] string? search,
                [FromQuery] int page,
                [FromQuery] int pageSize,
                [FromServices] IQueryHandler<ListSessionsQuery, ChatSessionsPageResponse> handler,
                CancellationToken ct) =>
            {
                var p = page <= 0 ? 1 : page;
                var s = pageSize <= 0 ? 20 : pageSize;
                var query = new ListSessionsQuery(status, search, p, s);
                var result = await handler.Handle(query, ct);
                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Chat)
            .RequireAuthorization("SuperAdminOnly");
    }
}
