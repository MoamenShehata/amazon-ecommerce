using Amazon.Orders.Domain.Orders.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Products;

// to maintain this it`s better to use event sourcing
public class Product : AuditableAggregate<Guid>, IEntity<Guid>
{
    public int InStockCount { get; private set; }
    public decimal CurrentPrice { get; private set; }

    public Product(Guid id, int inStockCount, decimal currentPrice) : base(id)
    {
        InStockCount = inStockCount;
        CurrentPrice = currentPrice;
    }

    public OrderItem CreateOrderItem(int quantity)
    {
        return new OrderItem(new ProductInstance(Id, CurrentPrice), quantity);
    }

    #region Infra
    private Product() : base(Guid.Empty) { }
    #endregion
}