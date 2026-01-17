using Amazon.Inventory.Domain.Products;
using Amazon.Inventory.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Inventory.Infrastructure.Data
{
    public class InventoryContext : DbContextBase
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<OutboxMessage> EventStore { get; private set; }

        public override bool AutoSaveDomainEvents => true;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products", "inventory");

                entity.ComplexProperty(x => x.Inventory, b =>
                {
                    b.Property(d => d.InStockCount);
                });

                entity.OwnsMany<ProductInventoryChange>("_inventoryChanges", b =>
                {
                    b.ToTable("inventoryChanges", "inventory");

                    b.WithOwner().HasForeignKey(x => x.ProductId);
                });
            });


            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("OutboxMessages", "inventory");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}