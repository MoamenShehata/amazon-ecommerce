using Amazon.SharedKernel.Orders.Commands;
using MassTransit;

namespace Amazon.Shipping.Application.EventConsumers;

public class CreateShipmentCommandHandler(ShippingAppService _shippingAppService) : IConsumer<CreateShipmentCommand>
{
    public async Task Consume(ConsumeContext<CreateShipmentCommand> context)
    {
        await _shippingAppService.CreateShipmentRequestAsync(context.Message);
    }
}
