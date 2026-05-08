using Amazon.Inventory.Domain.Products;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Inventory.Application.Products;

public class ProductAppService(IRepository<Product, Guid> _repository)
{
    public async Task<RestResponse<Product>> GetByIdAsync(Guid productId)
    {
        var product = await _repository.GetInstanceAsync(productId);
        if (product == null)
            return RestResponse<Product>.NotFound($"Product with ID {productId} not found.");

        return RestResponse<Product>.Success(product);
    }
}