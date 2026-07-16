using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;
using SharedKernel;

namespace Portfolio.Application.Projects.Reorder;

internal sealed class ReorderProjectsCommandHandler(IApplicationDbContext db) : ICommandHandler<ReorderProjectsCommand, ReorderResponse>
{
    public async Task<Result<ReorderResponse>> Handle(ReorderProjectsCommand cmd, CancellationToken ct)
    {
        var ids = cmd.Items.Select(i => i.Id).ToArray();
        var list = await db.Projects.Where(p => ids.Contains(p.Id)).ToListAsync(ct);
        var byId = cmd.Items.ToDictionary(i => i.Id, i => i.SortOrder);
        foreach (var p in list)
            if (byId.TryGetValue(p.Id, out var so)) p.SortOrder = so;
        await db.SaveChangesAsync(ct);
        return Result.Success(new ReorderResponse([.. ids]));
    }
}