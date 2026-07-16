using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application;
using Portfolio.Application.Abstractions.Presence;
using Shouldly;

namespace Portfolio.IntegratedTests;

public sealed class ApplicationDependencyInjectionTests
{
    [Fact]
    public void AddApplication_ShouldRegisterPresenceTrackerAsSingleton()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider = services.BuildServiceProvider();
        var first = provider.GetRequiredService<PresenceTracker>();
        var second = provider.GetRequiredService<PresenceTracker>();

        first.ShouldBeSameAs(second);
    }
}
