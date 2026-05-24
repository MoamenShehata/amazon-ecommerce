using Amazon.SharedKernel.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.SharedKernel.Extensions
{
    public static class DependencyRegistrarExtensions
    {
        public static IServiceCollection RegisterSharedServices(this IServiceCollection services)
        {
            services
                .AddScoped<IOtpService, OtpService>()
                ;

            return services;
        }
    }
}