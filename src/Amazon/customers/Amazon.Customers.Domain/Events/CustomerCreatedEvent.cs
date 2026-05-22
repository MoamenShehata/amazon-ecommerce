using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Customers.Domain.Events;

public record CustomerCreatedEvent(Guid CustomerId) : IntegrationEvent(DateTime.UtcNow);
