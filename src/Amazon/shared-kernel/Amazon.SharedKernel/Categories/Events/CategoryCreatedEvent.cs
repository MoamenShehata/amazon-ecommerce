using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Categories.Events;

public record CategoryCreatedEvent(Guid CategoryId) : IntegrationEvent(DateTime.UtcNow);
