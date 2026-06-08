using Amazon.Shipping.Domain.Events;
using MassTransit;

namespace Amazon.Shipping.Application.EventConsumers;

public class ShipmentRequestCreatedEventHandler(ShippingAppService _shippingAppService) : IConsumer<ShipmentRequestCreatedEvent>
{
    public async Task Consume(ConsumeContext<ShipmentRequestCreatedEvent> context)
    {
        await _shippingAppService.AssignShipmentToCompanyAsync(context.Message.Id);
    }
}
