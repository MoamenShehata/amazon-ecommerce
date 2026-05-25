using System.Reflection;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.Orders.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Orders.Infrastructure.Data;

public class OrdersContext : DbContextBase
{
    public OrdersContext(DbContextOptions<OrdersContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OutboxMessage> EventStore { get; private set; }

    public override bool AutoSaveDomainEvents => true;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("OutboxMessages", "orders");
        });

        modelBuilder.Entity<OrderCreatedStatus>();
        modelBuilder.Entity<OrderCancelledStatus>();
        modelBuilder.Entity<OrderProcessingStatus>();

        modelBuilder.Entity<OrderShippingStartedStatus>(b =>
        {
            b.OwnsOne(x => x.CompanyInfo);
        });

        modelBuilder.Entity<OrderShippedStatus>();


        modelBuilder.Entity<OrderDeliveryRecievedStatus>(b =>
        {
            b.OwnsOne(x => x.DeliveryMember);
        });

        modelBuilder.Entity<OrderDeliveredStatus>();

        base.OnModelCreating(modelBuilder);
    }
}