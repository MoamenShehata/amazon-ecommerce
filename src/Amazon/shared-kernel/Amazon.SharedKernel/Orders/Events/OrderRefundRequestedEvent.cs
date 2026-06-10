using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderRefundRequestedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);