using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderShippingStartedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);