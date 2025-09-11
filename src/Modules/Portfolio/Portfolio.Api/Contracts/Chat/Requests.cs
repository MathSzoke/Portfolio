namespace Portfolio.Api.Contracts.Chat;

public sealed class PostMessageRequest
{
    public string Content { get; init; } = null!;
    public bool AsMe { get; init; }
}

public sealed class StartSessionRequest
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
}
