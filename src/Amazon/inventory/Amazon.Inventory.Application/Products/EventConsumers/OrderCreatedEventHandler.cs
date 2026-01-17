using Amazon.Inventory.Domain.Orders;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Products.EventConsumers
{
    public class OrderCreatedEventHandler(
        OrdersService _ordersService,
        IUnitOfWork _unitOfWork) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderEvent = context.Message;

            var success = await _ordersService.UpdateInventoryForOrderAsync(orderEvent.OrderId, orderEvent.OrderItems);
            if (success)
                await _unitOfWork.CommitAsync();

            // negative ack
        }
    }
}
