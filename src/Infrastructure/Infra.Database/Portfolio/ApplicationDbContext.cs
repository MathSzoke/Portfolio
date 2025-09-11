using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Domain.Auth;
using Portfolio.Domain.Chats;
using Portfolio.Domain.Projects;
using Portfolio.Domain.Users;
using SharedKernel;
using SharedKernel.DomainEvents;

namespace Infra.Database.Portfolio;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ICurrentUserContext? currentUserContext,
    DomainEventsDispatcher domainEventsDispatcher)
    : BaseDbContext(options, currentUserContext, domainEventsDispatcher), IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<ExternalLogin> ExternalLogins => Set<ExternalLogin>();
    public DbSet<UserPhotos> UserPhotos => Set<UserPhotos>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ProjectAsset> ProjectAssets => Set<ProjectAsset>();
    public DbSet<ProjectTechnology> ProjectTechnologies => Set<ProjectTechnology>();
    public DbSet<ProjectTag> ProjectTags => Set<ProjectTag>();
    public DbSet<Showcase> Showcases => Set<Showcase>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<EmailOutbox> EmailOutbox => Set<EmailOutbox>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Portfolio.Infrastructure"));
        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}