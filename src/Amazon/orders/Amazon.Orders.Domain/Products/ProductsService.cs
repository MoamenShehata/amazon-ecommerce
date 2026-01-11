using Amazon.Orders.Domain.Orders;
using Moamen.SDKs.Repository;

namespace Amazon.Orders.Domain.Products
{
    public class ProductsService(IRepository<Product, Guid> _productsRepo)
    {
        public async Task<bool> IsAnyProductNotInventoryAvailableAsync(List<Guid> productIds)
        {
            return await _productsRepo.CountAsync(p => productIds.Contains(p.Id) && p.InStockCount == 0) > 0;
        }
    }
}