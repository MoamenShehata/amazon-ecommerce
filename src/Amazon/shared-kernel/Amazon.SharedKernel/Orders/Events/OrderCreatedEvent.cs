using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderCreatedEvent(Guid OrderId, List<KeyValuePair<Guid, int>> OrderItems) : IntegrationEvent(DateTime.UtcNow);