using Amazon.ProductCatalog.Infrastructure.Data;
using Amazon.ProductCatalog.Infrastructure.ReadModel;
using Amazon.ProductCatalog.Infrastructure.ReadModel.Services;
using Amazon.ProductCatalog.Read.Services;
using MassTransit;
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

            services
                .AddScoped<ICatalogReadServices, CatalogReadServices>()
                .AddDbContext<CatalogReadContext>(op => op.UseSqlServer(configuration.GetConnectionString("CatalogRead")));

            return services;
        }
    }
}
