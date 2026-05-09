using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.IntegrationEvents.ShoppingCart;

public record CartExpiredEvent(Guid[] ProductIds) : IntegrationEvent(DateTime.UtcNow);