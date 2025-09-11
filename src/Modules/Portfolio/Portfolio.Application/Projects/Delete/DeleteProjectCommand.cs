using MediatR;
using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.Delete;

public sealed record DeleteProjectCommand(Guid Id) : ICommand;