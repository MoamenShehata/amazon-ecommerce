using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Infrastructure.Data;
using EMP.SharedKernel;
using EMP.SharedKernel.Repositories;
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
            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CatalogDatabase"));
            });

            services
                .AddGenericRepos()
                .AddScoped(typeof(IRepository<,>), typeof(EfCoreRepositoryBase<,>))
                .AddScoped<DbContext>(sp => sp.GetRequiredService<CatalogDbContext>())
                .AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>())
                ;

        }
    }
}
