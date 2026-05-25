using Amazon.Inventory.Application.Products;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Orders.EventConsumers;

public class OrderCancelledEventHandler(
    ProductAppService _productsAppService,
    IUnitOfWork _unitOfWork) : IConsumer<OrderCancelledEvent>
{
    public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        await _productsAppService.ReleaseInventoryItemsForOrderAsync(context.Message.OrderId);
    }
}
