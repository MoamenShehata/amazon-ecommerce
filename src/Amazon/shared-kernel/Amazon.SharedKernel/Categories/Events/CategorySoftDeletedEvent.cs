using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Categories.Events;

public record CategorySoftDeletedEvent(Guid CategoryId, Guid? OrphanProductsNewCategoryId) : IntegrationEvent(DateTime.UtcNow);