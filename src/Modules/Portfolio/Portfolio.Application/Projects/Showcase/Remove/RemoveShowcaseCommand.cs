using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.Showcase.Remove;

public sealed record RemoveShowcaseCommand(string Slug) : ICommand;