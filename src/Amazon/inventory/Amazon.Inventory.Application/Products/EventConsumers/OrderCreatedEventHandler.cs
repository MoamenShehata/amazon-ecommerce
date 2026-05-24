using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Products.EventConsumers;

public class OrderCreatedEventHandler(
    ProductAppService _productsAppService,
    IUnitOfWork _unitOfWork) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderEvent = context.Message;

        await _productsAppService.LockAllForOrderAsync(orderEvent.OrderItems);
    }
}
