using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.ProductCatalog.Application
{
    public static class ApplicationDependencyRegistrar
    {
        public static void RegisterApplicationDependencies(this IServiceCollection services)
        {
            AddDomainServices(services);
        }

        private static void AddDomainServices(IServiceCollection services)
        {
            services
                .AddScoped<CategoriesService>()
                .AddScoped<ProductsService>()
                ;
        }
    }
}
