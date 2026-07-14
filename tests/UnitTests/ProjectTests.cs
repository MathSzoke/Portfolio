using Portfolio.Domain.Projects;
using Shouldly;

namespace Portfolio.UnitTests;

public sealed class ProjectTests
{
    [Fact]
    public void NewProject_ShouldStartWithNoRatingAndNoTechnologies()
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Portfolio",
            Summary = "Personal portfolio",
            ThumbnailUrl = "https://example.com/thumb.png",
            ProjectUrl = "https://example.com",
            RepoName = "Portfolio"
        };

        project.Rating.ShouldBe(0);
        project.RatingCount.ShouldBe(0);
        project.SortOrder.ShouldBe(0);
        project.Technologies.ShouldBeEmpty();
    }
}
