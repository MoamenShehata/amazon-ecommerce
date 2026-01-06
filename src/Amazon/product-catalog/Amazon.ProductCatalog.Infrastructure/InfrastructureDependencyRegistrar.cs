using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Infrastructure.Data;
using Amazon.ProductCatalog.Infrastructure.Interceptors;
using EMP.SharedKernel;
using EMP.SharedKernel.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.ProductCatalog.Infrastructure
{
    public static class InfrastructureDependencyRegistrar
    {
        public static void RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CatalogDatabase"));
                options.AddInterceptors(new DomainEventsPublisherInterceptor(sp.GetRequiredService<IMediator>()));
            });

            services
                .AddGenericRepos()
                .AddScoped<DbContext>(sp => sp.GetRequiredService<CatalogDbContext>())
                .AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>())
                ;

        }
    }
}
