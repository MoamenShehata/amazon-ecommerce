using System.Threading.Tasks;
using Moamen.SDKs.Repository;

namespace Amazon.Inventory.Domain.Products;

public class ProductService(IRepository<Product, Guid> _repository)
{
    public async Task ReserveProductItemsForOrderAsync(Guid orderId, List<KeyValuePair<Guid, int>> productsWithQuantities)
    {
        var products = await _repository.GetAllAsync(x => productsWithQuantities.Select(d => d.Key).Contains(x.Id));

        foreach (var product in products)
            product.ReserveQuantityForOrder(orderId,productsWithQuantities.FirstOrDefault(x => x.Key == product.Id).Value);
    }
}