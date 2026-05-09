using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.IntegrationEvents.ShoppingCart;

public record CartExpiredEvent(params Guid[] ProductIds) : IntegrationEvent(DateTime.UtcNow);