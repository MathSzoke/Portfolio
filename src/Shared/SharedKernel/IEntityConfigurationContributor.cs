using Microsoft.EntityFrameworkCore;

namespace SharedKernel;

public interface IEntityConfigurationContributor
{
    void ConfigureEntities(ModelBuilder modelBuilder);
}