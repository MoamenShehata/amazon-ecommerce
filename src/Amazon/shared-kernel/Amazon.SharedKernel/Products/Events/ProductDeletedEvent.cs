using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Products.Events;

public record ProductDeletedEvent(Guid ProductId) : IntegrationEvent(DateTime.UtcNow);