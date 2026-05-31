using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Infrastructure.Configurations.Carts;
using Amazon.Cart.Infrastructure.Configurations.Payments;
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
            modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentRequestConfiguration());

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