using Portfolio.AppHost;
using Portfolio.AppHost.Extensions.Database;
using Portfolio.AppHost.Extensions.Portfolio;

var builder = DistributedApplication.CreateBuilder(args);

var infra = builder.AddDatabase();
var projectsGroup = builder.AddGroup("Projects");


builder.AddPortfolioSuite(infra.Database, infra.Cache);

builder.Build().Run();