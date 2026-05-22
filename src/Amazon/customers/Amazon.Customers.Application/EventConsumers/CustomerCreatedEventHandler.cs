using Amazon.Customers.Application.CustomerProfiles;
using Amazon.Customers.Domain.Events;
using MassTransit;

namespace Amazon.Customers.Application.EventConsumers;

public class CustomerCreatedEventHandler(CustomerAppService _customerAppService) : IConsumer<CustomerCreatedEvent>
{
    public async Task Consume(ConsumeContext<CustomerCreatedEvent> context) => await _customerAppService.CreateProfileForCustomerAsync(context.Message.CustomerId);
}
