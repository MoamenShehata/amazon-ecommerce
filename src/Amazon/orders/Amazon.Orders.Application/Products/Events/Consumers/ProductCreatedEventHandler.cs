using Amazon.SharedKernel.IntegrationEvents.Products;
using MassTransit;

namespace Amazon.Orders.Application.Products.Events.Consumers
{
    public class ProductCreatedEventHandler : IConsumer<ProductCreatedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}