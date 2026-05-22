using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Customers.Domain.Events;

public record CustomerShippingInfoChangedEvent(Guid CustomerId) : IntegrationEvent(DateTime.UtcNow);