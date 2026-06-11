using Amazon.Cart.Domain.Products.ValueObjects;
using Amazon.Cart.Domain.ShoppingCarts.Entites;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;
using System.Text.Json.Serialization;

namespace Amazon.Cart.Domain.Products;

public class Product : AuditableAggregate<Guid>, IEntity<Guid>
{
    public ProductInfo Info { get; private set; }

    public Product(Guid id, ProductInfo info) : base(id)
    {
        Info = info;
    }

    public CartItem CreateCartItem() => new(this);
    public bool Satisfies(CartItem cartItem) => cartItem.ProductId == Id;

    public bool IsDeleted { get; private set; }
    public void SoftDelete() => IsDeleted = true;

    #region Infra
    private Product() : base(Guid.Empty)
    {

    }
    #endregion
}