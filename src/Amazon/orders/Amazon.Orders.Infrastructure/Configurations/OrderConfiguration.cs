using System.Text.Json;
using Amazon.Orders.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Orders.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", "orders");

        builder.ComplexProperty(x => x.Customer, b =>
        {
            b.Property(d => d.Id);
            b.Property(d => d.Email);
        });

        //builder.Property(x => x.Items).HasField("_orderItems");

        builder.OwnsMany(x => x.Items, b =>
        {
            b.WithOwner().HasForeignKey(x => x.OrderId);

            b.ToTable("orderItems", "orders");

            b.OwnsOne(x => x.ProductInfo, b =>
            {
                b.Property(d => d.ProductId);
                b.Property(d => d.UnitPrice);
                b.Property(d => d.Name);
            });
        });
    }
}

