using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.Orders.Infrastructure
{
    public static class ApplicationDependencyRegistrar
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services)
        {
            //services
            //    .AddGenericRepos()
            //    .AddScoped<DbContext>(sp => sp.GetRequiredService<CatalogDbContext>())
            //    .AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>())
            //    ;

            //services.AddMassTransit(config =>
            //{
            //    config.SetKebabCaseEndpointNameFormatter();

            //    config.AddConsumers(typeof(ApplicationDependencyRegistrar).Assembly);

            //    config.UsingRabbitMq((ctxt, configurator) =>
            //    {
            //        configurator.Host(new Uri(configuration["MessageBroker:Host"]), host =>
            //        {
            //            host.Username(configuration["MessageBroker:User"]);
            //            host.Password(configuration["MessageBroker:Password"]);
            //        });
            //        configurator.ConfigureEndpoints(ctxt);
            //    });
            //});

            return services;
        }

    }
}
