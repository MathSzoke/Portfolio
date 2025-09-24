using System.Reflection;
using HealthChecks.UI.Client;
using Infra.Database.Portfolio;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Portfolio.Api;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Api.Infrastructure.Hubs;
using Portfolio.Application;
using Portfolio.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, sp, cfg) => cfg
    .ReadFrom.Configuration(ctx.Configuration)
    .ReadFrom.Services(sp)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

builder.AddNpgsqlDbContext<ApplicationDbContext>("portfolioDB", configureDbContextOptions: options =>
{
    options.EnableDetailedErrors();
});

builder.Services.AddCorsServices(builder.Configuration);
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

UserDeviceCtx.Provider = app.Services;

app.MapEndpoints();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();
    app.MapOpenApi();
    app.ApplyMigrations();
}

app.ApplicationUses();

app.UseCorsApp();

app.MapControllers();

app.MapHub<PresenceHub>("/hubs/presenceHub").RequireAuthorization();

await app.RunAsync();

namespace Portfolio.Api
{
    public class Program;
}