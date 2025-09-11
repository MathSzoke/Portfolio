using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.Assets.Add;

public sealed record AddAssetCommand(Guid ProjectId, string Url, string? Title) : ICommand<AssetResponse>;
public sealed record AssetResponse(Guid Id, string Url, string? Title);