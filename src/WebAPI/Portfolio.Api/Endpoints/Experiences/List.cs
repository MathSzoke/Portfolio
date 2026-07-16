using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;

namespace Portfolio.Api.Endpoints.Experiences;

internal sealed class List : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/experiences", async (
                [FromQuery] string? language,
                [FromServices] IApplicationDbContext db,
                CancellationToken ct) =>
            {
                var items = await db.ExperienceItems
                    .AsNoTracking()
                    .OrderBy(x => x.SortOrder)
                    .ThenByDescending(x => x.StartDate)
                    .ToListAsync(ct);

                return Results.Ok(items.Select(x => x.ToResponse(language ?? "pt-BR")));
            })
            .WithTags(Tags.Experiences);
    }
}
