using System.Text.Json;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Tests
{
    public class InMemoryContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Orders");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(x => x.Items).HasField("_orderItems")
            .HasConversion(
            ls => JsonSerializer.Serialize(ls, (JsonSerializerOptions)null),
            json => JsonSerializer.Deserialize<List<OrderItem>>(json, (JsonSerializerOptions)null));
        }

        public override int SaveChanges()
        {
            var auditableEntities = ChangeTracker.Entries<IAuditableEntity>().Select(x => x.Entity);
            foreach (var item in auditableEntities)
            {
                item.CreatedOn = DateTime.UtcNow;
                item.CreatedBy = "asd";
            }

            return base.SaveChanges();
        }
    }
}