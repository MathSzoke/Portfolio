using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using SharedKernel;

namespace Portfolio.Application.Projects.Assets.Remove;

internal sealed class RemoveAssetCommandHandler(IApplicationDbContext db) : ICommandHandler<RemoveAssetCommand>
{
    public async Task<Result> Handle(RemoveAssetCommand cmd, CancellationToken ct)
    {
        var p = await db.Projects.Include(x => x.Assets).FirstOrDefaultAsync(x => x.Id == cmd.ProjectId, ct);
        if (p is null) return Result.Failure(Portfolio.Domain.Projects.ProjectErrors.NotFound(cmd.ProjectId));
        var asset = p.Assets.FirstOrDefault(a => a.Id == cmd.AssetId);
        if (asset is null) return Result.Failure(Portfolio.Domain.Projects.ProjectErrors.AssetNotFound(cmd.AssetId));
        p.Assets.Remove(asset);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}