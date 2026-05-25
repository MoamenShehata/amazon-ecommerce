using Amazon.Inventory.Application.Products;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Orders.EventConsumers;

public class OrderShippingStartedEventHandler(
    ProductAppService _productsAppService,
    IUnitOfWork _unitOfWork) : IConsumer<OrderShippingStartedEvent>
{
    public async Task Consume(ConsumeContext<OrderShippingStartedEvent> context)
    {
        await _productsAppService.ConsumeProductItemsFromInventoryAsync(context.Message.OrderId);
    }
}
