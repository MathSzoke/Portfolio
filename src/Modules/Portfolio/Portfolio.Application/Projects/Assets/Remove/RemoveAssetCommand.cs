using MediatR;
using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.Assets.Remove;

public sealed record RemoveAssetCommand(Guid ProjectId, Guid AssetId) : ICommand;