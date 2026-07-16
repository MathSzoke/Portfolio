using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Contracts.Experiences;
using Portfolio.Application.Abstractions.Data;

namespace Portfolio.Api.Endpoints.Experiences;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/experiences/{id:guid}", async (
                [FromRoute] Guid id,
                [FromBody] ExperienceItemRequest request,
                [FromServices] IApplicationDbContext db,
                IServiceProvider services,
                CancellationToken ct) =>
            {
                var item = await db.ExperienceItems.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (item is null)
                {
                    return Results.NotFound();
                }

                await item.ApplyRequestAsync(request, services, ct);
                await db.SaveChangesAsync(ct);

                return Results.Ok(item.ToResponse(request.SourceLanguage));
            })
            .WithTags(Tags.Experiences)
            .RequireAuthorization("SuperAdminOnly");
    }
}
