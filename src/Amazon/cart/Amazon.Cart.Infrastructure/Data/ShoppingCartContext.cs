using Amazon.Cart.Domain;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Infrastructure.Data
{
    public class ShoppingCartContext : DbContextBase
    {
        public DbSet<ShoppingCart> Carts { get; set; }

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>(e =>
            {
                e.ComplexProperty(c => c.Expiration, b =>
                {
                    b.Property(p => p.ExpiresAt).HasColumnName(nameof(ShoppingCart.Expiration.ExpiresAt));
                });

                e.OwnsMany(x => x.Items).WithOwner().HasForeignKey(x => x.ShoppingCartId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}