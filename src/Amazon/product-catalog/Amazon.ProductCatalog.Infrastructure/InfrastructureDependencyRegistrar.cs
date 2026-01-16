using Amazon.ProductCatalog.Infrastructure.Data;
using Amazon.ProductCatalog.Infrastructure.Interceptors;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;
using Moamen.SDKs.SharedKernel;

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
                .AddOutboxServices()
                .AddBaseContext<CatalogDbContext>()
                ;

            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                //if (subscribersAssembly != null)
                //    config.AddConsumers(subscribersAssembly);

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
        }
    }
}
