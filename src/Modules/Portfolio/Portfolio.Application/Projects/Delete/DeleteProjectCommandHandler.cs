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
        await db.Projects.Where(p => p.Id == cmd.Id).ExecuteDeleteAsync(ct);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}