using Amazon.Inventory.Application.Products;
using Amazon.SharedKernel.Orders.Commands;
using MassTransit;

namespace Amazon.Inventory.Application.Orders.EventConsumers;

public class ReserveInventoryCommandHandler(
    ProductAppService _productsAppService) : IConsumer<ReserveInventoryCommand>
{
    public async Task Consume(ConsumeContext<ReserveInventoryCommand> context)
    {
        var orderEvent = context.Message;

        await _productsAppService.ReserveProductItemsForOrderAsync(orderEvent.OrderId, orderEvent.OrderItems);
    }
}