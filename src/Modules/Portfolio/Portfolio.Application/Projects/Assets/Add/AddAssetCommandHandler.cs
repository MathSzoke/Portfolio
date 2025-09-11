using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects;
using Portfolio.Domain.Projects.Enums;
using SharedKernel;

namespace Portfolio.Application.Projects.Assets.Add;

internal sealed class AddAssetCommandHandler(IApplicationDbContext db)
    : ICommandHandler<AddAssetCommand, AssetResponse>
{
    public async Task<Result<AssetResponse>> Handle(AddAssetCommand cmd, CancellationToken ct)
    {
        var p = await db.Projects.Include(x => x.Assets).FirstOrDefaultAsync(x => x.Id == cmd.ProjectId, ct);
        if (p is null) return Result.Failure<AssetResponse>(Portfolio.Domain.Projects.ProjectErrors.NotFound(cmd.ProjectId));
        var order = p.Assets.Count;
        var asset = new ProjectAsset { Url = cmd.Url, Title = cmd.Title, Kind = AssetKind.Image, SortOrder = order };
        p.Assets.Add(asset);
        await db.SaveChangesAsync(ct);
        return Result.Success(new AssetResponse(asset.Id, asset.Url, asset.Title));
    }
}