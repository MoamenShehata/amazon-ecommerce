using Amazon.Orders.Domain.Orders.ValueObjects;

namespace Amazon.Orders.Domain.Orders.Factories;

public class ProductInstanceFactory
{
    private static List<ProductInstance> _pool = new();

    public ProductInstance Create(Guid productId, decimal unitPrice)
    {
        var cached = _pool.FirstOrDefault(x => x.ProductId == productId && x.UnitPrice == unitPrice);
        if(cached != null)
            return cached;

        var instance = new ProductInstance(productId, unitPrice);
        _pool.Add(instance);

        return instance;
    }
}