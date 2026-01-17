using Amazon.ProductCatalog.Application.Categories;
using Amazon.ProductCatalog.Application.Products;
using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.ProductCatalog.Application
{
    public static class ApplicationDependencyRegistrar
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services)
        {
            AddDomainServices(services);
            AddApplicationServices(services);

            return services;
        }

        private static void AddDomainServices(IServiceCollection services)
        {
            services
                .AddScoped<CategoriesService>()
                .AddScoped<ProductsService>()
                ;
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            //services.AddMediatR(config =>
            //{
            //    config.RegisterServicesFromAssembly(typeof(ApplicationDependencyRegistrar).Assembly);
            //});

            services
                .AddScoped<CategoriesAppService>()
                .AddScoped<ProductsAppService>()
                ;
        }
    }
}
