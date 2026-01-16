namespace Amazon.SharedKernel.IntegrationEvents.Orders;

public record OrderCreatedIntegrationEvent(DateTime OccurredOn, Guid OrderId, List<KeyValuePair<Guid, int>> OrderItems);