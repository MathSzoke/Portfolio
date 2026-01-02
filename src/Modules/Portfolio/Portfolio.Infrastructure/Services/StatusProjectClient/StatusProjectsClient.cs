using System.Net.Http.Json;

namespace Portfolio.Infrastructure.Services.StatusProjectClient;

internal sealed class StatusProjectsClient(HttpClient client) : IStatusProjectsClient
{
    public async Task RegisterAsync(string name, string urlEndpoint, string urlRedirect, CancellationToken ct)
    {
        var payload = new
        {
            Name = name,
            UrlEndpoint = urlEndpoint,
            UrlRedirect = urlRedirect
        };

        var resp = await client.PostAsJsonAsync("/registerProject", payload, ct);
        resp.EnsureSuccessStatusCode();
    }