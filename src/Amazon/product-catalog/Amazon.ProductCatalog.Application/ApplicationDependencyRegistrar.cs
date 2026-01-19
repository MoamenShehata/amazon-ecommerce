using Amazon.ProductCatalog.Application.Categories;
using Amazon.ProductCatalog.Application.Products;
using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Read.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.ProductCatalog.Application
{
    public static class ApplicationDependencyRegistrar
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            AddDomainServices(services);
            AddApplicationServices(services);

            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.AddConsumers(typeof(ProductForListModel).Assembly);

                config.UsingRabbitMq((ctxt, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]), host =>
                    {
                        host.Username(configuration["MessageBroker:User"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });
                    configurator.ConfigureEndpoints(ctxt);
                });
            });

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
