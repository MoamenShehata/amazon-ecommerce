using Amazon.SharedKernel.Jobs;
using Amazon.SharedKernel.Jobs.RabbitMq;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.SharedKernel.Extensions
{
    public static class BackgroundJobsExtensions
    {
        public static IServiceCollection AddSharedJobs(this IServiceCollection services)
        {
            services
                .AddScoped<EventsPublishService>()
                .AddJob<IntegrationEventsPublishJob>();

            return services;
        }

        public static IServiceCollection AddJob<TJob>(this IServiceCollection services) where TJob : BackgroundJobBase
        {
            services.AddHostedService<TJob>();

            return services;
        }

    }
}