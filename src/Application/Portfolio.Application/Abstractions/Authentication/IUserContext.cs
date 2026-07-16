namespace Portfolio.Application.Abstractions.Authentication;

public interface IUserContext
{
    string UserId { get; }
    Guid UserID { get; }
}