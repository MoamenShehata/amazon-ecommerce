using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Shipping.Domain.Events;

internal record ShipmentAssignedToCompanyEvent(Guid OrderId, Guid ShipmentRequestId, Guid ShippingCompanyId) : IntegrationEvent(DateTime.UtcNow);