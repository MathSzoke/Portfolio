using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;

namespace Portfolio.Application.User.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;