using Media.Application.Storage;
using Media.Infrastructure.Data;
using Media.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Media.Infrastructure
{
    public static class InfrastructureDependencyRegistrar
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddGenericRepos()
                .AddBaseContext<MediaContext>(op => op.UseSqlServer(configuration.GetConnectionString("Default")))
                ;

            services.AddSingleton<IStorageService, StorageService>();

            return services;
        }

    }
}
