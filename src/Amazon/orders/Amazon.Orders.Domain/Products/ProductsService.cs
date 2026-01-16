using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Orders.Domain.Products
{
    public class ProductsService(IRepository<Product, Guid> _productsRepo)
    {
        public async Task<RestResponse<bool>> ValidateProducts(List<Guid> productIds)
        {
            var products = (await _productsRepo.GetAllAsync(p => productIds.Contains(p.Id), x => new { x.Id, x.InStockCount })).ToList();
            if (products.Count == 0)
                return RestResponse<bool>.NotFound("Some of the products were not found!");

            if (products.Count(p => p.InStockCount == 0) > 0)
                return RestResponse<bool>.BadRequest(new BadRequestModel("Cannot full fill this whole order request due to lack of some products in inventory"));

            return RestResponse<bool>.Success(true);
        }

        public void Create(Guid id, string name, int inStockCount, decimal currentPrice)
        {
            var product = new Product(id, name, inStockCount, currentPrice);
            _productsRepo.Add(product);
        }
    }
}