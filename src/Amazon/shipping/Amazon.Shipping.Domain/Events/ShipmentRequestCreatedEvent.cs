using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Shipping.Domain.Events;

public record ShipmentRequestCreatedEvent(Guid Id) : IntegrationEvent(DateTime.UtcNow);