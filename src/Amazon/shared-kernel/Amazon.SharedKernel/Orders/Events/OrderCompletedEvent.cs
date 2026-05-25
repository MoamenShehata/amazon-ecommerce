using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderCompletedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);