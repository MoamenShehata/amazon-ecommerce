using Amazon.ProductCatalog.Application.Categories;
using Amazon.SharedKernel.Jobs;

namespace Amazon.ProductCatalog.Api.Jobs
{
    public class CategoriesSoftDeleteJob(
        ILogger<CategoriesSoftDeleteJob> _logger,
        IServiceScopeFactory _serviceScopeFactory
        ) : BackgroundJobBase(_logger)
    {
        protected override TimeSpan Interval => TimeSpan.FromSeconds(5);

        protected override async Task DoAsync(CancellationToken stoppingToken)
        {
            var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<CategoriesAppService>();
            await service.SoftDeleteCategories();
        }
    }

}