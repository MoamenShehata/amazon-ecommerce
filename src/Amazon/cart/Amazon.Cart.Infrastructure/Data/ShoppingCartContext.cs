using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Entities;
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

                e.OwnsMany(x => x.Items, b =>
                {
                    b.WithOwner().HasForeignKey(x => x.ShoppingCartId);

                    b.HasKey(x => new { x.ShoppingCartId, x.Id });

                    b.Property(x=>x.Id).ValueGeneratedNever();
                });

                e.Navigation(o => o.Items).HasField("_cartItems")
             .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            //modelBuilder.Entity<CartItem>(e =>
            //{
            //    e.HasKey(x => new { x.ShoppingCartId, x.Id });
            //});

            base.OnModelCreating(modelBuilder);
        }
    }
}