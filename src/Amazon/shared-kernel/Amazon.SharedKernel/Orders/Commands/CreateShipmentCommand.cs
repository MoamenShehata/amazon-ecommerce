using Amazon.SharedKernel.Customers;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Commands;

public record CreateShipmentCommand(Guid OrderId, CustomerInfo Customer, DeliveryAddress DeliverToAddress) : IntegrationEvent(DateTime.UtcNow);
public record ShipmentCreatedEvent(Guid OrderId, Guid ShipmentId) : IntegrationEvent(DateTime.UtcNow);
public record CreateShipmentFailedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);