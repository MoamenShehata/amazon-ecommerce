using Amazon.Inventory.Domain.Products;
using Amazon.Inventory.Domain.Products.Entities;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Products;

public class ProductAppService(
    IRepository<Product, Guid> _repository,
    IUnitOfWork _unitOfWork)
{
    public async Task<RestResponse<Product>> GetByIdAsync(Guid productId)
    {
        var product = await _repository.GetInstanceAsync(productId);
        if (product == null)
            return RestResponse<Product>.NotFound($"Product with ID {productId} not found.");

        return RestResponse<Product>.Success(product);
    }

    public async Task<RestResponse<int>> HoldItemForPurchaseAsync(Guid productId)
    {
        var productResult = await GetByIdAsync(productId);
        if (!productResult.IsSuccess)
            return productResult.MapTo(0);

        var holdItemIdResult = productResult.Value.Inventory.HoldItemForPurchase();
        if (!holdItemIdResult.IsSuccess)
            return holdItemIdResult.MapTo(0);

        await _unitOfWork.CommitAsync();
        return RestResponse<int>.Success(holdItemIdResult.Value);
    }

    public async Task ReleaseProductsOnHoldAsync(params Guid[] productIds)
    {
        var productsOnHold = await _repository.GetAllAsync(x => productIds.Contains(x.Id));
        foreach (var item in productsOnHold)
            item.Inventory.ReleaseAllOnHoldItems();

        await _unitOfWork.CommitAsync();
    }
}