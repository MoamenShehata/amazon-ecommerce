using Amazon.Customers.Domain.Events;
using MassTransit;

namespace Amazon.Customers.Application.EventConsumers;

public class CustomerShippingInfoChangedEventHandler(CustomerAppService _customerAppService) : IConsumer<CustomerShippingInfoChangedEvent>
{
    public async Task Consume(ConsumeContext<CustomerShippingInfoChangedEvent> context) => await _customerAppService.UpdateProfileAddressesForCustomerAsync(context.Message.CustomerId);
}