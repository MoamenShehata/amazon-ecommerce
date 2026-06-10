using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderAbandonedEvent(Guid OrderId, string Reason) : IntegrationEvent(DateTime.UtcNow);