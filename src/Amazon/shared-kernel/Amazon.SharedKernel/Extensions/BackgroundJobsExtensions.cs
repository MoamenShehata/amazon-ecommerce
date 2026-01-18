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
                .AddHostedService<IntegrationEventsPublishJob>();

            return services;
        }
    }
}