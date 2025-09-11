using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ProjectTechnologyConfig : IEntityTypeConfiguration<ProjectTechnology>
{
    public void Configure(EntityTypeBuilder<ProjectTechnology> b)
    {
        b.HasKey(x => new { x.ProjectId, x.TechnologyId });

        b.HasOne(x => x.Project)
            .WithMany(x => x.Technologies)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Technology)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.TechnologyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        b.HasQueryFilter(s => !s.IsDeleted);
    }
}