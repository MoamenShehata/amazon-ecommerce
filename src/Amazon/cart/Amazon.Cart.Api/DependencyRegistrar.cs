using Amazon.Cart.Api.Jobs;
using Amazon.Cart.Api.Services;
using Amazon.Cart.Application;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Application.Settings.PaymentGateways;
using Amazon.Cart.Infrastructure;
using Amazon.SharedKernel.Extensions;

namespace Amazon.Cart.Api;

public static class DependencyRegistrar
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFrameworkServices()
            .AddAuthorizationPolicies()
            ;

        services
            .AddSharedJobs()
            .RegisterSharedServices(configuration)
            .RegisterOtpServices()
            .AddJob<PurgeExpiredCartsJob>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddHttpContextAccessor()
            .AddAppSettings()

            .RegisterApplicationDependencies(configuration)
            .RegisterInfrastructureDependencies(configuration);

        services.AddJwtAuthentication(configuration);

        return services;
    }

    private static IServiceCollection AddFrameworkServices(this IServiceCollection services)
    {

        services.AddControllers();
        services.AddOpenApi();

        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(op =>
        {
            op.AddPolicy("CARTS_POLICY", builder =>
            {
                builder.RequireClaim("scope", "amazon.cart");
            });

            op.AddPolicy("CUSTOMERS_POLICY", builder =>
            {
                builder.RequireClaim("scope", "amazon.customers");
            });
        });


        return services;
    }

    private static IServiceCollection AddAppSettings(this IServiceCollection services)
    {
        services
            .AddOptions<PaymentGatewaySettings>()
            .BindConfiguration(PaymentGatewaySettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}