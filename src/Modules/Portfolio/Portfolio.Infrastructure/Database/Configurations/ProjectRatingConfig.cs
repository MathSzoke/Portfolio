using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ProjectRatingConfig : IEntityTypeConfiguration<ProjectRating>
{
    public void Configure(EntityTypeBuilder<ProjectRating> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Value)
            .IsRequired();

        b.HasIndex(x => new { x.ProjectId, x.UserId })
            .IsUnique();

        b.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}