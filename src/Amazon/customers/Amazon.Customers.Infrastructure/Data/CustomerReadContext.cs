using Amazon.Customers.Application.CustomerProfiles.Models;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;
using System.Text.Json;

namespace Amazon.Customers.Infrastructure.Data;

public class CustomerReadContext : DbContextBase
{
    public override bool AutoSaveDomainEvents => true;
    public CustomerReadContext(DbContextOptions<CustomerReadContext> options) : base(options)
    {
    }

    public DbSet<CustomerProfile> CustomerProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("read");

        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.ToTable("customerProfiles");

            entity.HasKey(x => x.CustomerId);

            entity.Property(x => x.Addresses)
            .HasConversion(
                list => JsonSerializer.Serialize(list, (JsonSerializerOptions)null),
                json => JsonSerializer.Deserialize<ICollection<CustomerProfileAddress>>(json, (JsonSerializerOptions)null));
        });

        base.OnModelCreating(modelBuilder);
    }
}
