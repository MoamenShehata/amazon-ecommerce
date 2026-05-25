using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderCancelledEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);