using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.Rate;

public sealed record RateProjectCommand(Guid ProjectId, float Rating) : ICommand<RatedProjectResponse>;