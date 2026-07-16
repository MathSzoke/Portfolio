using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Auth;
using Portfolio.Domain.Projects;
using Portfolio.Domain.Chats;
using Portfolio.Domain.Users;

namespace Portfolio.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Domain.Users.User> Users { get; }
    DbSet<ExternalLogin> ExternalLogins { get; }
    DbSet<UserPhotos> UserPhotos { get; }
    DbSet<Project> Projects { get; }
    DbSet<Technology> Technologies { get; }
    DbSet<ProjectTechnology> ProjectTechnologies { get; }
    DbSet<ProjectRating> ProjectRatings { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<ChatSession> ChatSessions { get; }
    DbSet<ChatMessage> ChatMessages { get; }
    DbSet<EmailOutbox> EmailOutbox { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}