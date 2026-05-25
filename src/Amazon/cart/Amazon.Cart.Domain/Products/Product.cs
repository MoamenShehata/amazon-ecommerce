using Amazon.Cart.Domain.Products.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Products;

public class Product : AuditableAggregate<Guid>, IEntity<Guid>
{
    public ProductInfo Info { get; private set; }

    public Product(Guid id, ProductInfo productInfo) : base(id)
    {
        Info = productInfo;
    }

    #region Infra

    private Product() : base(Guid.Empty)
    {
        
    }
    #endregion
}