using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;

namespace Portfolio.Application.Projects.Showcase.Replace;

public sealed record ReplaceShowcaseCommand(string Slug, ShowcaseDto Showcase) : ICommand;