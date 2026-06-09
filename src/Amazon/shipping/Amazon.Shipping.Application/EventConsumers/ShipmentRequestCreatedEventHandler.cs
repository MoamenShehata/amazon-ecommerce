using Amazon.SharedKernel.Orders.Commands;
using MassTransit;

namespace Amazon.Shipping.Application.EventConsumers;

public class ShipmentRequestCreatedEventHandler(ShippingAppService _shippingAppService) : IConsumer<ShipmentCreatedEvent>
{
    public async Task Consume(ConsumeContext<ShipmentCreatedEvent> context)
    {
        await _shippingAppService.AssignShipmentToCompanyAsync(context.Message.ShipmentId);
    }
}
