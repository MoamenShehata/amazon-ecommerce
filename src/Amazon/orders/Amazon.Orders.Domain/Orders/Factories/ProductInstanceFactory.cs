using Amazon.Orders.Domain.Orders.ValueObjects;

namespace Amazon.Orders.Domain.Orders.Factories;

public class ProductInstanceFactory
{
    //should handle concurrency
    private static List<ProductInstance> _cache = new();

    public ProductInstance Create(Guid productId, decimal unitPrice)
    {
        var cached = _cache.FirstOrDefault(x => x.ProductId == productId && x.UnitPrice == unitPrice);
        if (cached != null)
            return cached;

        var instance = new ProductInstance(productId, unitPrice);

        //should handle concurrency
        _cache.Add(instance);

        return instance;
    }
}