using System.Text.Json;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.SharedKernel.Customers;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Orders.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", "orders");

        builder.ComplexProperty(x => x.Owner, b =>
        {
            b.Property(d => d.Id);
            b.Property(d => d.Email);
            b.Property(d => d.PhoneNumber);
        });

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

        builder.Property(x => x.DeliveryAddress)
            .HasConversion(
            address => JsonSerializer.Serialize(address, null as JsonSerializerOptions),
            json => JsonSerializer.Deserialize<DeliveryAddress>(json, null as JsonSerializerOptions)
            );

        builder.Navigation(o => o.Items).HasField("_orderItems")
         .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany<OrderStatusChange>("_history")
            .WithOne()
            .HasForeignKey(x => x.OrderId);

        builder.Navigation("_history").AutoInclude();
    }
}

