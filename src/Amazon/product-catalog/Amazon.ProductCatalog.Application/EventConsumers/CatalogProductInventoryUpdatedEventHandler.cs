using Amazon.ProductCatalog.Domain.Products;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.ProductCatalog.Application.EventConsumers;

public class CatalogProductInventoryUpdatedEventHandler(
    IRepository<Product, Guid> _productsRepository,
    IUnitOfWork _unitOfWork) : IConsumer<ProductInventoryUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductInventoryUpdatedEvent> context)
    {
        var product = await _productsRepository.GetInstanceAsync(context.Message.ProductId);
        product.UpdateStockCount(context.Message.CurrentInventory);
        await _unitOfWork.CommitAsync();
    }
}
