namespace SharedKernel;

public interface ICurrentUserContext
{
    string? UserId { get; }
    Guid UserIdGuid { get; }
}