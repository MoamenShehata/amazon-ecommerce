using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.Products.Events.Consumers;

public class ProductInventoryUpdatedIntegrationEventHandler(
    ProductsService _productsService,
    IUnitOfWork _unitOfWork) : IConsumer<ProductInventoryUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductInventoryUpdatedEvent> context)
    {
        var message = context.Message;

        await _productsService.UpdateInventoryAsync(message.ProductId, message.CurrentInventory);
        await _unitOfWork.CommitAsync();
    }
}