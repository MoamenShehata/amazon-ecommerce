using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Infrastructure.Data;
using Amazon.ProductCatalog.Infrastructure.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.ProductCatalog.Infrastructure
{
    public static class InfrastructureDependencyRegistrar
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddGenericRepos()
                .AddBaseContext<CatalogDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("CatalogDatabase")));
            ;

            services.AddHttpClient();

            services
                .AddScoped<IMediaService, ProductMediaService>()
                ;

            return services;
        }
    }
}
