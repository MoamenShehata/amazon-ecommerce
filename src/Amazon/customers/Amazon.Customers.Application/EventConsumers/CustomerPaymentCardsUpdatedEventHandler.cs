using Amazon.Customers.Domain.Events;
using MassTransit;

namespace Amazon.Customers.Application.EventConsumers;

public class CustomerPaymentCardsUpdatedEventHandler(CustomerAppService _customerAppService) : IConsumer<CustomerPaymentCardsUpdatedEvent>
{
    public async Task Consume(ConsumeContext<CustomerPaymentCardsUpdatedEvent> context) => await _customerAppService.UpdateProfilePaymentCardsAsync(context.Message.CustomerId);
}
