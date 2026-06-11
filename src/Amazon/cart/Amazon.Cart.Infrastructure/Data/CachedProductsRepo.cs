using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.Data.NoSql;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System.Text.Json;

namespace Amazon.Cart.Infrastructure.Data;

internal class CachedProductsRepo : MongoDbRepository<Product, Guid>, IProductsRepo
{
    private readonly IDistributedCache _cache;

    public CachedProductsRepo(
        IMongoDatabase _database,
        IDistributedCache cache) : base(_database, "products")
    {
        _cache = cache;
    }

    public override async Task<Product> GetInstanceAsync(Guid id)
    {
        var instanceKey = GenerateProductCahceKey(id);

        var cachedProduct = await _cache.GetStringAsync(instanceKey);
        if (!string.IsNullOrWhiteSpace(cachedProduct))
            return JsonSerializer.Deserialize<Product>(cachedProduct);

        var product = await base.GetInstanceAsync(id);
        await _cache.SetStringAsync(instanceKey, JsonSerializer.Serialize(product));

        return product;
    }

    public async Task<List<Product>> GetCartProductsAsync(ShoppingCart shoppingCart)
    {
        var result = new List<Product>();

        var productIds = shoppingCart.Items.Select(x => x.ProductId);
        foreach (var productId in productIds)
            result.Add(await GetInstanceAsync(productId));

        return result;
    }

    private string GenerateProductCahceKey(Guid productId) => productId.ToString();
}