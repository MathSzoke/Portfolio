using MediatR;
using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.Reorder;

public sealed record ReorderProjectsCommand(IReadOnlyCollection<Item> Items) : ICommand;
public sealed record Item(Guid Id, int SortOrder);