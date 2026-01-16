namespace Amazon.SharedKernel.IntegrationEvents.Products
{
    public record ProductCreatedIntegrationEvent(DateTime OccurredOn, Guid ProductId, string Name, decimal UnitPrice);
}