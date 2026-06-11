using Amazon.Cart.Domain.Products;
using Amazon.SharedKernel.Data.NoSql;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System.Text.Json;

namespace Amazon.Cart.Infrastructure.Data;

internal class CachedProductsRepo : MongoDbRepository<Product, Guid>
{
    private readonly IDistributedCache _cache;

    public CachedProductsRepo(
        IMongoDatabase _database,
        MongoDbRepository<Product, Guid> _repository,
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

    private string GenerateProductCahceKey(Guid productId) => productId.ToString();
}