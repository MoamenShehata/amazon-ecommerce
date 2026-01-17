using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.IntegrationEvents.Products;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.Products.Events.Consumers;

public class ProductInventoryUpdatedIntegrationEventHandler(
    ProductsService _productsService,
    IUnitOfWork _unitOfWork) : IConsumer<ProductInventoryUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductInventoryUpdatedIntegrationEvent> context)
    {
        var message = context.Message;

        await _productsService.UpdateInventoryAsync(message.ProductId, message.CurrentInventory);
        await _unitOfWork.CommitAsync();
    }
}