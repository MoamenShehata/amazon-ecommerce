using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Data.NoSql;
using Amazon.SharedKernel.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using MongoDB.Driver;
using Serilog;

namespace Amazon.SharedKernel.Extensions
{
    public static class DependencyRegistrarExtensions
    {
        public static IServiceCollection RegisterSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<ISmsService, SmsService>()
                .AddScoped<ITextGenerator, TextGenerator>()
                .AddScoped<ITextServices, TextServices>()
                ;

            services
                .AddSerilog((services, lc) =>
                {
                    lc.Enrich.FromLogContext();
                    lc.WriteTo.Console();
                    lc.ReadFrom.Configuration(configuration);
                });

            services.RegisterHttpClientServices();

            return services;
        }

        public static IServiceCollection AddMongoDb(this IServiceCollection services, string connectionString, string dataBaseName)
        {
            services
                .AddSingleton<IMongoClient>(sp =>
                {
                    return new MongoClient(connectionString);
                })
                .AddSingleton(sp =>
                {
                    var client = sp.GetRequiredService<IMongoClient>();

                    return client.GetDatabase(dataBaseName);
                });

            return services;
        }

        public static IServiceCollection AddMongoRepo<TCollection, TKey>(this IServiceCollection services, string collectionName)
            where TCollection : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            services.AddScoped(sp => new MongoDbRepository<TCollection, TKey>(sp.GetRequiredService<IMongoDatabase>(), collectionName));

            services.AddScoped<IRepository<TCollection, TKey>>(sp => sp.GetRequiredService<MongoDbRepository<TCollection, TKey>>());

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
                    options.Authority = configuration.GetValue<string>("JwtSettings:Authority");
                    options.Audience = configuration.GetValue<string>("JwtSettings:Audience");
                    options.RequireHttpsMetadata = false;
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
                            Console.WriteLine(context.Exception.Message);
                            Console.WriteLine(context.Exception.StackTrace);
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            Console.WriteLine(context.Result.Failure.Message);
                            Console.WriteLine(context.Result.Failure.StackTrace);
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