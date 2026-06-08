using Amazon.Inventory.Domain.Products;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Products;

public class ProductAppService(
    IRepository<Product, Guid> _repository,
    ProductService _productsService,
    IUnitOfWork _unitOfWork)
{
    public async Task<RestResponse<Product>> GetByIdAsync(Guid productId)
    {
        return await _productsService.IsProductInStockAsync(productId);
    }

    public async Task ReserveProductItemsForOrderAsync(Guid orderId, List<KeyValuePair<Guid, int>> orderItems)
    {
        await _productsService.ReserveProductItemsForOrderAsync(orderId, orderItems);
        await _unitOfWork.CommitAsync();
    }

    public async Task ReleaseInventoryItemsForOrderAsync(Guid orderId)
    {
        var orderProducts = await _repository.GetAllAsync(p => p.Inventory.Items.Any(i => i.ReservedForOrder == orderId));
        foreach (var product in orderProducts)
            product.ReleaseReservedItems();

        await _unitOfWork.CommitAsync();
    }

    public async Task ConsumeProductItemsFromInventoryAsync(Guid orderId)
    {
        var orderProducts = await GetProductsForOrderAsync(orderId);
        foreach (var product in orderProducts)
            product.ConsumeForOrder(orderId);

        await _unitOfWork.CommitAsync();
    }

    private async Task<IEnumerable<Product>> GetProductsForOrderAsync(Guid orderId) => await _repository.GetAllAsync(p => p.Inventory.Items.Any(i => i.ReservedForOrder == orderId));
}