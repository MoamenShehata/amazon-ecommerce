using Amazon.SharedKernel.Customers.Events;
using MassTransit;

namespace Amazon.Customers.Application.EventConsumers;

public class NewCustomerRegistrationEventHandler(CustomerAppService _customerAppService) : IConsumer<NewCustomerRegistrationEvent>
{
    public async Task Consume(ConsumeContext<NewCustomerRegistrationEvent> context)
    {
        await _customerAppService.CreateCustomerAsync(context.Message);
    }
}