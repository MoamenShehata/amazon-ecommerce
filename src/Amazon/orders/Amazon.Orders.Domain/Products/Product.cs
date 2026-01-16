using Amazon.Orders.Domain.Orders.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Products;

// to maintain this it`s better to use event sourcing
public class Product : AuditableAggregate<Guid>, IEntity<Guid>
{
    public string Name { get; private set; }
    public int InStockCount { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public Product(Guid id, string name, int inStockCount, decimal currentPrice) : base(id)
    {
        Name = name;
        InStockCount = inStockCount;
        CurrentPrice = currentPrice;
    }

    public OrderItem CreateOrderItem(Guid orderId, int quantity)
    {
        return new OrderItem(orderId, new ProductInfo(Id, CurrentPrice, Name), quantity);
    }

    #region Infra
    private Product() : base(Guid.Empty) { }
    #endregion
}