using Portfolio.AppHost;
using Portfolio.AppHost.Extensions.Database;
using Portfolio.AppHost.Extensions.Portfolio;

var builder = DistributedApplication.CreateBuilder(args);

var infra = builder.AddDatabase();
var projectsGroup = builder.AddGroup("Projects");

var moduleFrontends = new Dictionary<string, IResourceBuilder<NodeAppResource>>();

builder.AddPortfolioSuite(infra.Database, infra.Cache);

builder.Build().Run();