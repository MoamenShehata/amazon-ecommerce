using Amazon.Cart.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Cart.Infrastructure.Configurations.Carts;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasQueryFilter(x => x.Expiration.ExpiresAt > DateTime.UtcNow);

        builder.ComplexProperty(c => c.Expiration, b =>
        {
            b.Property(p => p.ExpiresAt).HasColumnName(nameof(ShoppingCart.Expiration.ExpiresAt));
        });

        builder.OwnsMany(x => x.Items, b =>
        {
            b.WithOwner().HasForeignKey(x => x.ShoppingCartId);

            b.HasKey(x => new { x.ShoppingCartId, x.Id });

            b
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);

            b.Navigation(x => x.Product).AutoInclude();
        });

        builder.Navigation(o => o.Items)
            .HasField("_cartItems")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}