using Amazon.ProductCatalog.Application.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.ProductCatalog.Api.Jobs
{
    public class CategoriesSoftDeleteJob(
        ILogger<CategoriesSoftDeleteJob> _logger,
        IServiceScopeFactory _serviceScopeFactory
        ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CategoriesSoftDeleteJob started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background work running at {time}", DateTime.UtcNow);

                var scope = _serviceScopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<CategoriesAppService>();
                await service.SoftDeleteCategories();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("CategoriesSoftDeleteJob stopped");
        }
    }

}