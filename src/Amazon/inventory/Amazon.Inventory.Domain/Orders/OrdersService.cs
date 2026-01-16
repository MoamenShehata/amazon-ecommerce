using Amazon.Inventory.Domain.Products;
using Moamen.SDKs.Repository;

namespace Amazon.Inventory.Domain.Orders
{
    public class OrdersService(IRepository<Product, Guid> _productsRepository)
    {
        public async Task<bool> UpdateInventoryForOrderAsync(Guid orderId, List<KeyValuePair<Guid, int>> orderItems)
        {
            var products = await _productsRepository.GetAllAsync(p => orderItems.Select(x => x.Key).Contains(p.Id));

            Func<Product, bool> productInventoryConsumer = p => p.ConsumeForOrder(orderItems.FirstOrDefault(x => x.Key == p.Id).Value).IsSuccess;

            return products.Select(productInventoryConsumer).ToList().All(x => true);
        }
    }
}