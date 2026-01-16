using System.Reflection;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Infrastructure.Data;

public class OrdersContext : DbContextBase
{
    public OrdersContext(DbContextOptions<OrdersContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}