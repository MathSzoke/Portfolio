using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects;
using SharedKernel;
using System;

namespace Portfolio.Application.Projects.Rate;

internal sealed class RateProjectCommandHandler(IApplicationDbContext db, ICurrentUserContext current)
    : ICommandHandler<RateProjectCommand, RatedProjectResponse>
{
    public async Task<Result<RatedProjectResponse>> Handle(RateProjectCommand cmd, CancellationToken ct)
    {
        var userId = current.UserIdGuid;
        if (userId == Guid.Empty) return Result.Failure<RatedProjectResponse>(ProjectErrors.Unauthorized);

        var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == cmd.ProjectId, ct);
        if (project is null) return Result.Failure<RatedProjectResponse>(ProjectErrors.NotFound(cmd.ProjectId));

        var value = Math.Clamp(cmd.Rating, 0f, 5f);

        var existing = await db.ProjectRatings.FirstOrDefaultAsync(r => r.ProjectId == cmd.ProjectId && r.UserId == userId, ct);
        if (existing is null)
        {
            var rating = new ProjectRating
            {
                Id = Guid.NewGuid(),
                ProjectId = cmd.ProjectId,
                UserId = userId,
                Value = value
            };
            db.ProjectRatings.Add(rating);
        }
        else
        {
            existing.Value = value;
        }

        await db.SaveChangesAsync(ct);

        var stats = await db.ProjectRatings
            .Where(r => r.ProjectId == cmd.ProjectId)
            .GroupBy(r => r.ProjectId)
            .Select(g => new { Avg = g.Average(x => x.Value), Count = g.Count() })
            .SingleOrDefaultAsync(ct);

        project.Rating = stats?.Avg ?? 0f;
        project.RatingCount = stats?.Count ?? 0;

        await db.SaveChangesAsync(ct);

        return Result.Success(new RatedProjectResponse(project.Rating, project.RatingCount));
    }
}
