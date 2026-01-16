using Amazon.Orders.Domain.Orders.ValueObjects;

namespace Amazon.Orders.Domain.Orders.Factories;

public class ProductInstanceFactory
{
    //should handle concurrency
    private static List<ProductInfo> _cache = new();

    public ProductInfo Create(Guid productId, decimal unitPrice, string name)
    {
        var cached = _cache.FirstOrDefault(x => x.ProductId == productId && x.UnitPrice == unitPrice);
        if (cached != null)
            return cached;

        var instance = new ProductInfo(productId, unitPrice, name);

        //should handle concurrency
        _cache.Add(instance);

        return instance;
    }
}