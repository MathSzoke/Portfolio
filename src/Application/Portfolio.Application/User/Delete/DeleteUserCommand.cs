using Portfolio.Application.Abstractions.Messaging;
using SharedKernel;

namespace Portfolio.Application.User.Delete;

public sealed record DeleteUserCommand : ICommand<Result>;