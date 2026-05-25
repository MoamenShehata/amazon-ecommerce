using Amazon.Inventory.Application.Products;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Orders.EventConsumers;

public class OrderCreatedEventHandler(
    ProductAppService _productsAppService,
    IUnitOfWork _unitOfWork) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderEvent = context.Message;

        await _productsAppService.ReserveProductItemsForOrderAsync(orderEvent.OrderId, orderEvent.OrderItems);
    }
}