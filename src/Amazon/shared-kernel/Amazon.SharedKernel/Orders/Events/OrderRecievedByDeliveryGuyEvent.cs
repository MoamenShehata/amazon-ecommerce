using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderRecievedByDeliveryGuyEvent(Guid OrderId, string Name, string Phone) : IntegrationEvent(DateTime.UtcNow);
