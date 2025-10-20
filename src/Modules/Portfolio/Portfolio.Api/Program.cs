using HealthChecks.UI.Client;
using Infra.Database.Portfolio;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Portfolio.AIAgent;
using Portfolio.Api;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Api.Infrastructure.Hubs;
using Portfolio.Application;
using Portfolio.Infrastructure;
using Serilog;
using System.Reflection;

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

builder.Services.AddAiAgent(builder.Configuration);

builder.AddNpgsqlDbContext<ApplicationDbContext>("portfolioDB", configureDbContextOptions: options =>
{
    options.EnableDetailedErrors();
});

builder.Services.AddCorsServices(builder.Configuration);
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

UserDeviceCtx.Provider = app.Services;

app.MapEndpoints();

app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));
app.MapHealthChecks("health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}); 

app.UseSwaggerWithUi();
app.MapOpenApi();
app.ApplyMigrations();

app.ApplicationUses();

app.UseCorsApp();

app.MapControllers();

app.MapHubs();

await app.RunAsync();

namespace Portfolio.Api
{
    public class Program;
}