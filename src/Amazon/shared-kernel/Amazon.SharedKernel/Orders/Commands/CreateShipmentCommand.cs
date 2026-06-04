using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Commands;

public record CreateShipmentCommand(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);
public record ShipmentCreatedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);
public record CreateShipmentFailedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);