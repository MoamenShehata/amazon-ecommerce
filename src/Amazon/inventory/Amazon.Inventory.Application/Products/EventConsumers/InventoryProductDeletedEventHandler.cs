using Amazon.Inventory.Domain.Products;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Products.EventConsumers;

public class InventoryProductDeletedEventHandler(
IRepository<Product, Guid> _productsRepo,
IUnitOfWork _unitOfWork) : IConsumer<ProductDeletedEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        var productEvent = context.Message;

        var product = await _productsRepo.GetInstanceAsync(productEvent.ProductId);
        product.SoftDelete();

        await _unitOfWork.CommitAsync();
    }
}