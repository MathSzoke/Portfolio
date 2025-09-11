namespace Portfolio.AppHost.Extensions.Database;

internal static class DatabaseExtensions
{
    public static InfrastructureResources AddDatabase(this IDistributedApplicationBuilder builder)
    {
        var postgresDbGroup = builder.AddGroup("PostgresDB");

        var cache = builder.AddRedis("cache");

        var pgPassword = builder.AddParameter("postgres-password", true);

        var postgresServer = builder.AddPostgres("postgres-server")
            .WithPassword(pgPassword)
            .WithDataVolume("postgres_data")
            .WithPgAdmin()
            .WithAnnotation(new ResourceRelationshipAnnotation(postgresDbGroup.Resource, "Parent"));

        var sharedDb = postgresServer.AddDatabase("portfolioDB");

        return new InfrastructureResources(cache, sharedDb);
    }

    public record InfrastructureResources(
        IResourceBuilder<RedisResource> Cache,
        IResourceBuilder<PostgresDatabaseResource> Database
    );
}