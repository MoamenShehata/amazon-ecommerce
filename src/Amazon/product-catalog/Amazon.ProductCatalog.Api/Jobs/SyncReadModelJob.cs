using Amazon.ProductCatalog.Application.Categories;
using Amazon.ProductCatalog.Read.Services;
using Amazon.SharedKernel.Products.Events;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Api.Jobs
{
    public class SyncReadModelJob(
        ILogger<SyncReadModelJob> _logger,
        IServiceScopeFactory _serviceScopeFactory
        ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SyncReadModelJob started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background work running at {time}", DateTime.UtcNow);

                var scope = _serviceScopeFactory.CreateScope();
                var categoryAppService = scope.ServiceProvider.GetRequiredService<CategoriesAppService>();
                var eventStoreService = scope.ServiceProvider.GetRequiredService<EventStoreService>();
                var service = scope.ServiceProvider.GetRequiredService<ICatalogReadServices>();

                var events = await eventStoreService.GetEventsAsync<ProductCreatedEvent>();

                foreach (var productCreatedEvent in events)
                {
                    // needs refactoring
                    var category = await categoryAppService.GetByIdAsync(productCreatedEvent.CategoryId);

                    var categories = category.Value.Name + "," + string.Join(",", category.Value.Children);

                    await service.InsertProductAsync(productCreatedEvent.ProductId, productCreatedEvent.Name, categories, productCreatedEvent.UnitPrice);
                }


                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("SyncReadModelJob stopped");
        }
    }
}
