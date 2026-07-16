namespace Portfolio.Application.Abstractions.Projects;

public interface IStatusProjectsClient
{
    Task RegisterAsync(string name, string urlEndpoint, string urlRedirect, CancellationToken ct);
}