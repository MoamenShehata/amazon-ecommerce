using Amazon.Customers.Domain;
using Amazon.Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Customers.Infrastructure.Data;

public class CustomersContext : DbContextBase
{
    public override bool AutoSaveDomainEvents => true;
    public CustomersContext(DbContextOptions<CustomersContext> options) : base(options)
    {
    }

    public DbSet<OutboxMessage> EventStore { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");

            entity.OwnsOne(x => x.ContactInfo, b =>
            {
                b.Property(x => x.Email).IsRequired();
                b.Property(x => x.PhoneNumber).IsRequired();
            });


            entity.OwnsOne(x => x.ShippingInfo, b =>
            {
                b.OwnsMany(s => s.ShippingAddresses, sa =>
                {
                    sa.WithOwner().HasForeignKey(x => x.CustomerId);

                    sa.OwnsOne(x => x.City, b =>
                    {
                        b.Property(x => x.CountryId).IsRequired();
                        b.Property(x => x.CityId).IsRequired();
                        b.Property(x => x.PostalCode).IsRequired();
                    });

                    sa.OwnsOne(x => x.House, b =>
                    {
                        b.Property(x => x.Street).IsRequired();
                        b.Property(x => x.BuildingNumber).IsRequired();
                        b.Property(x => x.ApartmentNumber).IsRequired(false);
                    });

                    sa.Property(x => x.IsDefault).IsRequired();
                });
            });
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("OutboxMessages");
        });

        modelBuilder.Entity<PaymentCard>(entity =>
        {
            entity.ToTable("PaymentCards");

            entity.HasOne<Customer>()
                .WithMany(c => c.PaymentCards)
                .HasForeignKey(pc => pc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.ComplexProperty(x => x.Info, b =>
            {
                b.Property(x => x.HolderName);

                b.ComplexProperty(x => x.Number, bb =>
                {
                    bb.Property(x => x.Value).HasColumnName("Number").HasField("_cardNumber");
                });

                b.ComplexProperty(x => x.Expiration, bb =>
                {
                    bb.Property(x => x.Month);
                    bb.Property(x => x.Year);
                });
            });

            entity.ComplexProperty(x => x.State, b =>
            {
                b.Property(x => x.IsActive);
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}