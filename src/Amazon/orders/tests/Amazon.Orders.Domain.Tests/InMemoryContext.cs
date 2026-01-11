using System.Text.Json;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Microsoft.EntityFrameworkCore;

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
    }
}