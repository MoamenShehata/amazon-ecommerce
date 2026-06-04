using Amazon.Inventory.Application.Products;
using Amazon.SharedKernel.Orders.Commands;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Orders.EventConsumers;

public class OrderCreatedEventHandler(
    ProductAppService _productsAppService,
    IUnitOfWork _unitOfWork) : IConsumer<ReserveInventoryCommand>
{
    public async Task Consume(ConsumeContext<ReserveInventoryCommand> context)
    {
        var orderEvent = context.Message;

        await _productsAppService.ReserveProductItemsForOrderAsync(orderEvent.OrderId, orderEvent.OrderItems);
    }
}