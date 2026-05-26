using Amazon.Cart.Api.Jobs;
using Amazon.Cart.Api.Services;
using Amazon.Cart.Application;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Infrastructure;
using Amazon.SharedKernel.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Amazon.Cart.Api;

public static class DependencyRegistrar
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSharedJobs()
            .RegisterSharedServices()
            .AddJob<PurgeExpiredCartsJob>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddHttpContextAccessor()

            .RegisterApplicationDependencies(configuration)
            .RegisterInfrastructureDependencies(configuration);

        services.AddJwtAuthentication(configuration);

        return services;
    }


    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.UseSecurityTokenValidators = false;
                options.Authority = configuration.GetValue<string>("JwtSettings:Issuer");
                options.Audience = configuration.GetValue<string>("JwtSettings:Audience");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetValue<string>("JwtSettings:Issuer"),
                    ValidAudiences = [configuration.GetValue<string>("JwtSettings:Audience")],

                };
            });

        return services;
    }
}