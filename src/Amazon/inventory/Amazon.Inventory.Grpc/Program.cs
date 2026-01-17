using Amazon.Inventory.Application;
using Amazon.Inventory.Grpc.Services;
using Amazon.Inventory.Infrastructure;
using Amazon.SharedKernel.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterInfrastructureDependencies(builder.Configuration);

builder.Services.AddHostedService<IntegrationEventsPublishJob>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
