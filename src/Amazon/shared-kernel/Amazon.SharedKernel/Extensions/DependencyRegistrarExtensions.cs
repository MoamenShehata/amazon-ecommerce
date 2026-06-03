using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Amazon.SharedKernel.Extensions
{
    public static class DependencyRegistrarExtensions
    {
        public static IServiceCollection RegisterSharedServices(this IServiceCollection services)
        {
            services
                .AddScoped<ISmsService, SmsService>()
                .AddScoped<ITextGenerator, TextGenerator>()
                .AddScoped<ITextServices, TextServices>()
                ;

            services.RegisterHttpClientServices();

            return services;
        }
        
        public static IServiceCollection RegisterOtpServices(this IServiceCollection services)
        {
            services
                .AddScoped<IOtpService, OtpService>()
                ;

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
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
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetValue<string>("JwtSettings:Issuer"),
                        ValidAudiences = [configuration.GetValue<string>("JwtSettings:Audience")],
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            return Task.CompletedTask;
                        },
                    };
                });

            return services;
        }

        private static void RegisterHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services
                .AddTransient<InjectJwtFromCurrentSessionHandler>()
                .AddTransient<HttpClientErrorHandler>()
                ;
        }
    }
}