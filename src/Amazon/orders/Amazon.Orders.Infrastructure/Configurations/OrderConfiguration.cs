using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.Entites;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.SharedKernel.Customers;
using Amazon.SharedKernel.Orders.Events;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

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

        builder.OwnsMany<Transaction>("_transactions", b =>
        {
            b.WithOwner().HasForeignKey(x => x.OrderId);

            b.ToTable("payments", "orders");

            b.Property(x => x.Type);

            b.Property(x => x.IsArchived);

            b.Property(x => x.PaymentInfo)
            .HasConversion(
                o => JsonSerializer.Serialize(o, null as JsonSerializerOptions),
                json => JsonSerializer.Deserialize<CheckoutPaymentInfo>(json, null as JsonSerializerOptions)
            );
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

