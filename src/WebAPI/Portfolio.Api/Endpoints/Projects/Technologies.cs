using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Domain.Projects;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class Technologies : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/Technologies", async (
            IApplicationDbContext db,
            CancellationToken ct) =>
        {
            var technologies = await db.Technologies
                .AsNoTracking()
                .ToListAsync(ct);

            return Results.Ok(technologies);
        })
        .WithTags(Tags.Projects)
        .RequireAuthorization("SuperAdminOnly");

        app.MapPost("api/v1/Technologies", async (
            IApplicationDbContext db,
            Technology request,
            CancellationToken ct) =>
        {
            var tech = new Technology
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await db.Technologies.AddAsync(tech, ct);
            await db.SaveChangesAsync(ct);

            return Results.Ok(tech);
        })
        .WithTags(Tags.Projects)
        .RequireAuthorization("SuperAdminOnly");
    }
}
