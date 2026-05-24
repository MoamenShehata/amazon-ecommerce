using System.Threading.Tasks;
using Moamen.SDKs.Repository;

namespace Amazon.Inventory.Domain.Products;

public class ProductService(IRepository<Product, Guid> _repository)
{
    public async Task LockAllForOrderAsync(List<KeyValuePair<Guid, int>> productsWithQuantities)
    {
        var products = await _repository.GetAllAsync(x => productsWithQuantities.Select(d => d.Key).Contains(x.Id));

        foreach (var product in products)
            product.ReserveQuantityForOrder(productsWithQuantities.FirstOrDefault(x => x.Key == product.Id).Value);
    }
}