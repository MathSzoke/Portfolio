using Infra.Database.Portfolio;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        appDbContext.Database.Migrate();
    }
}