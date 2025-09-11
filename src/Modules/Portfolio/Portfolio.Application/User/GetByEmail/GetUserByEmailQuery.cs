using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;

namespace Portfolio.Application.User.GetByEmail;

public sealed record GetUserByEmailQuery(string email) : IQuery<UserResponse>;