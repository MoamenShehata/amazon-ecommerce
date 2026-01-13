namespace Amazon.SharedKernel.IntegrationEvents.Products
{
    public record ProductCreatedIntegrationEvent(DateTime occurredOn, Guid ProductId, string Name, decimal UnitPrice);
}