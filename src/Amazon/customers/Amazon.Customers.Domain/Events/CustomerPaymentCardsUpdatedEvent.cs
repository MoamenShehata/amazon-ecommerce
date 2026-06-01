using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Customers.Domain.Events;

public record CustomerPaymentCardsUpdatedEvent(Guid CustomerId) : IntegrationEvent(DateTime.UtcNow);
