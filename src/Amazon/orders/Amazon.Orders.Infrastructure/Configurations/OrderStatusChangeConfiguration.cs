using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Orders.Infrastructure.Configurations;

public class OrderStatusChangeConfiguration : IEntityTypeConfiguration<OrderStatusChange>
{
    public void Configure(EntityTypeBuilder<OrderStatusChange> builder)
    {
        builder.ToTable("statusHistory", "orders");


        builder.Property(x => x.Id).UseIdentityColumn();
        builder.HasKey(x => new { x.OrderId, x.Id });
        //builder.HasKey(x => x.OrderId);

        builder.UseTphMappingStrategy();
    }
}
