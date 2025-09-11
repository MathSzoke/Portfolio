using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ProjectTagConfig : IEntityTypeConfiguration<ProjectTag>
{
    public void Configure(EntityTypeBuilder<ProjectTag> b)
    {
        b.HasKey(x => new { x.ProjectId, x.TagId });

        b.HasOne(x => x.Project)
            .WithMany(x => x.Tags)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Tag)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Restrict);
        
        b.HasQueryFilter(s => !s.IsDeleted);
    }
}