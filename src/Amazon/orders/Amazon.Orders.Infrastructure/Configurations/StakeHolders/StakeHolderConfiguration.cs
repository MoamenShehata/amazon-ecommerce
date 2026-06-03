using Amazon.Orders.Domain.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Orders.Infrastructure.Configurations.StakeHolders;

public class StakeHolderConfiguration : IEntityTypeConfiguration<StakeHolder>
{
    public void Configure(EntityTypeBuilder<StakeHolder> builder)
    {
        builder.UseTphMappingStrategy();
    }
}
