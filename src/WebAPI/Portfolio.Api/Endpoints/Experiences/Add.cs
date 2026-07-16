using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Contracts.Experiences;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Domain.Experiences;

namespace Portfolio.Api.Endpoints.Experiences;

internal sealed class Add : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/experiences", async (
                [FromBody] ExperienceItemRequest request,
                [FromServices] IApplicationDbContext db,
                IServiceProvider services,
                CancellationToken ct) =>
            {
                var item = new ExperienceItem
                {
                    Id = Guid.NewGuid(),
                    SortOrder = request.SortOrder == 0
                        ? await db.ExperienceItems.CountAsync(ct) + 1
                        : request.SortOrder
                };

                await item.ApplyRequestAsync(request with { SortOrder = item.SortOrder }, services, ct);
                db.ExperienceItems.Add(item);
                await db.SaveChangesAsync(ct);

                return Results.Ok(item.ToResponse(request.SourceLanguage));
            })
            .WithTags(Tags.Experiences)
            .RequireAuthorization("SuperAdminOnly");
    }
}
