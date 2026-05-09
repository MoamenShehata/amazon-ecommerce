using Amazon.Inventory.Domain.Products;
using Amazon.Inventory.Domain.Products.Entities;
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
            modelBuilder.HasDefaultSchema("inventory");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.OwnsOne(x => x.Inventory, b =>
                {
                    b.OwnsMany<InventoryItem>("_items", b =>
                    {
                        b.ToTable("inventoryItems");

                        b.WithOwner().HasForeignKey(x => x.ProductId);
                    });
                });

                entity.OwnsMany<ProductInventoryChange>("_inventoryChanges", b =>
                {
                    b.ToTable("inventoryChanges");

                    b.WithOwner().HasForeignKey(x => x.ProductId);
                });
            });


            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("OutboxMessages");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}