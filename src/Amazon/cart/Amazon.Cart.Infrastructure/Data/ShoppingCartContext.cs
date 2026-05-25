using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Cart.Infrastructure.Data
{
    public class ShoppingCartContext : DbContextBase
    {
        public DbSet<ShoppingCart> Carts { get; set; }
        public override bool AutoSaveDomainEvents => true;

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>(e =>
            {
                e.HasQueryFilter(x => x.Expiration.ExpiresAt > DateTime.UtcNow);

                e.ComplexProperty(c => c.Expiration, b =>
                {
                    b.Property(p => p.ExpiresAt).HasColumnName(nameof(ShoppingCart.Expiration.ExpiresAt));
                });

                e.OwnsMany(x => x.Items, b =>
                {
                    b.WithOwner().HasForeignKey(x => x.ShoppingCartId);

                    b.HasKey(x => new { x.ShoppingCartId, x.Id });

                    b
                    .HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId);

                    b.Navigation(x => x.Product).AutoInclude();
                });

                e.Navigation(o => o.Items).HasField("_cartItems")
             .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("OutboxMessages");
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.OwnsOne(x => x.Info);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}