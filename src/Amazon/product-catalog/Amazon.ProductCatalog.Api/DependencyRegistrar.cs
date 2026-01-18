using Amazon.ProductCatalog.Api.Jobs;
using Amazon.ProductCatalog.Application;
using Amazon.ProductCatalog.Infrastructure;
using Amazon.SharedKernel.Extensions;
using Microsoft.OpenApi.Models;

namespace Amazon.ProductCatalog.Api;

public static class DependencyRegistrar
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services
            .AddSharedJobs()
            .AddHostedService<CategoriesSoftDeleteJob>()
            .AddHostedService<SyncReadModelJob>()
            ;

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Amazon Product Catalog API",
                Version = "v1",
                Description = "API for product catalog management"
            });
        });

        builder.Services
            .RegisterApplicationDependencies()
            .RegisterInfrastructureDependencies(builder.Configuration)
            .AddSharedJobs();

    }
}