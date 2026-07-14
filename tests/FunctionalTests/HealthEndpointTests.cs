using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Portfolio.Api;
using Shouldly;

namespace Portfolio.FunctionalTests;

public sealed class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Secret"] = "local-functional-tests-secret-key-with-enough-length",
                    ["Jwt:Issuer"] = "Portfolio.FunctionalTests",
                    ["Jwt:Audience"] = "Portfolio.FunctionalTests",
                    ["ConnectionStrings:portfolioDB"] = "Host=localhost;Database=portfolio_tests;Username=postgres;Password=postgres"
                });
            });
        });
    }

    [Fact]
    public async Task Healthz_ShouldReturnOkLocally()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/healthz");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
