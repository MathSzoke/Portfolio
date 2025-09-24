using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects;
using SharedKernel;

namespace Portfolio.Application.Projects.Delete;

internal sealed class DeleteProjectCommandHandler(IApplicationDbContext db) : ICommandHandler<DeleteProjectCommand>
{
    public async Task<Result> Handle(DeleteProjectCommand cmd, CancellationToken ct)
    {
        var entity = await db.Projects.FirstOrDefaultAsync(p => p.Id == cmd.Id, ct);
        if (entity is null) return Result.Failure(ProjectErrors.NotFound(cmd.Id));
        db.Projects.Remove(entity);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}