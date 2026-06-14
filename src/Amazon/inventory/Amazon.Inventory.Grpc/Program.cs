using Amazon.Inventory.Grpc;
using Amazon.Inventory.Grpc.Services;
using Amazon.Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<InventoryContext>();
await ctx.Database.MigrateAsync();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductGrpService>();
app.MapGrpcReflectionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
