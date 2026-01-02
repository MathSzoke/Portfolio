namespace Portfolio.Infrastructure.Services.StatusProjectClient;

public interface IStatusProjectsClient
{
    Task RegisterAsync(string name, string urlEndpoint, string urlRedirect, CancellationToken ct);
}