using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;

namespace Portfolio.Api.Endpoints.Experiences;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/experiences/{id:guid}", async (
                [FromRoute] Guid id,
                [FromServices] IApplicationDbContext db,
                CancellationToken ct) =>
            {
                var item = await db.ExperienceItems.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (item is null)
                {
                    return Results.NotFound();
                }

                db.ExperienceItems.Remove(item);
                await db.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .WithTags(Tags.Experiences)
            .RequireAuthorization("SuperAdminOnly");
    }
}
