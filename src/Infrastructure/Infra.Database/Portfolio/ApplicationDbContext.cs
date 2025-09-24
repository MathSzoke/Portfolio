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
    public DbSet<User> Users => this.Set<User>();
    public DbSet<ExternalLogin> ExternalLogins => this.Set<ExternalLogin>();
    public DbSet<UserPhotos> UserPhotos => this.Set<UserPhotos>();
    public DbSet<Project> Projects => this.Set<Project>();
    public DbSet<Technology> Technologies => this.Set<Technology>();
    public DbSet<ProjectTechnology> ProjectTechnologies => this.Set<ProjectTechnology>();
    public DbSet<ProjectRating> ProjectRatings => this.Set<ProjectRating>();
    public DbSet<RefreshToken> RefreshTokens => this.Set<RefreshToken>();
    public DbSet<ChatSession> ChatSessions => this.Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => this.Set<ChatMessage>();
    public DbSet<EmailOutbox> EmailOutbox => this.Set<EmailOutbox>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Portfolio.Infrastructure"));
        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}