using Amazon.SharedKernel.Customers;
using Amazon.Shipping.Domain;
using Amazon.Shipping.Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;
using System.Reflection;
using System.Text.Json;

namespace Amazon.Orders.Infrastructure.Data;

public class ShippingContext : DbContextBase
{
    public ShippingContext(DbContextOptions<ShippingContext> options) : base(options) { }

    public override bool AutoSaveDomainEvents => true;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("shipping");

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("OutboxMessages");
        });

        modelBuilder.Entity<ShipmentRequest>(b =>
        {
            b.ToTable("requests");

            b.Property(x => x.Customer)
            .HasConversion(o => JsonSerializer.Serialize(o, null as JsonSerializerOptions),
            json => JsonSerializer.Deserialize<CustomerInfo>(json, null as JsonSerializerOptions));

            b.Property(x => x.ToAddress)
            .HasConversion(o => JsonSerializer.Serialize(o, null as JsonSerializerOptions),
            json => JsonSerializer.Deserialize<DeliveryAddress>(json, null as JsonSerializerOptions));


            b
            .HasOne<ShippingCompany>()
            .WithMany()
            .HasForeignKey(x => x.AssignedToCompanyId);
        });

        modelBuilder.Entity<ShippingCompany>(b =>
        {
            b.ToTable("companies");
        });

        base.OnModelCreating(modelBuilder);
    }
}