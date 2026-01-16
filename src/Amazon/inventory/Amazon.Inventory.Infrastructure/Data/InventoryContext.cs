using Amazon.Inventory.Domain.Products;
using Amazon.Inventory.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Infrastructure.Data
{
    public class InventoryContext : DbContextBase
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}