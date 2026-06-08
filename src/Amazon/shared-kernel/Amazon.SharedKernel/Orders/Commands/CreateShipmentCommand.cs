using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Commands;

public record CreateShipmentCommand(Guid OrderId, Guid CustomerId, string CustomerEmail, string CustomerPhone, string DeliverToAddressJson) : IntegrationEvent(DateTime.UtcNow);
public record ShipmentCreatedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);
public record CreateShipmentFailedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);