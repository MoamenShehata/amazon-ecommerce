using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Orders.Domain.Products
{
    public class ProductsService(IRepository<Product, Guid> _productsRepo)
    {
        public async Task<RestResponse<bool>> ValidateProducts(List<KeyValuePair<Guid, int>> orderItems)
        {
            var products = (await _productsRepo.GetAllAsync(p => orderItems.Select(x => x.Key).Contains(p.Id), x => new { x.Id, x.InStockCount })).ToList();
            if (products.Count != orderItems.Count)
                return RestResponse<bool>.NotFound("Some of the products were not found!");

            if (products.Any(p => p.InStockCount < orderItems.FirstOrDefault(x => x.Key == p.Id).Value))
                return RestResponse<bool>.BadRequest("Cannot full fill this whole order request due to lack of some products in inventory");

            return RestResponse<bool>.Success(true);
        }

        public void Create(Guid id, string name, int inStockCount, decimal currentPrice)
        {
            var product = new Product(id, name, inStockCount, currentPrice);
            _productsRepo.Add(product);
        }

        public async Task UpdateInventoryAsync(Guid id, int newInventory)
        {
            var product = await _productsRepo.GetInstanceAsync(id);
            if (product == null) return;

            product.UpdateInStockCount(newInventory);
        }
    }
}