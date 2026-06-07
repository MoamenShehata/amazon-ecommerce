using MassTransit;
using Media.Domain.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Media.Application
{
    public static class ApplicationDependencyRegistrar
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.AddConsumers(typeof(ApplicationDependencyRegistrar).Assembly);

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

            services
                .AddScoped<MediaService>()
                .AddScoped<MediaFactory>()
                .AddScoped<MediaAccessibilityFactory>()
                .AddScoped<MediaAuthenticationKeyFactory>()
                ;

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            });

            return services;
        }
    }
}
