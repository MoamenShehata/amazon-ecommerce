using Amazon.Cart.Api.Jobs;
using Amazon.Cart.Api.Services;
using Amazon.Cart.Application;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Infrastructure;
using Amazon.SharedKernel.Extensions;

namespace Amazon.Cart.Api;

public static class DependencyRegistrar
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSharedJobs()
            .RegisterSharedServices()
            .RegisterOtpServices()
            .AddJob<PurgeExpiredCartsJob>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddHttpContextAccessor()

            .RegisterApplicationDependencies(configuration)
            .RegisterInfrastructureDependencies(configuration);

        services.AddJwtAuthentication(configuration);

        return services;
    }
}