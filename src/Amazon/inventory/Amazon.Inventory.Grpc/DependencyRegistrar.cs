using Amazon.Inventory.Application;
using Amazon.Inventory.Infrastructure;
using Amazon.SharedKernel.Extensions;

namespace Amazon.Inventory.Grpc;

public static class DependencyRegistrar
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddGrpc();
        
        builder.Services
            .AddGrpcReflection();

        builder.Services
            .RegisterApplicationDependencies(builder.Configuration)
            .RegisterInfrastructureDependencies(builder.Configuration)
            .AddSharedJobs();

    }
}