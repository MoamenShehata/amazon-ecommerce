using Amazon.Inventory.Grpc.Services;
using Amazon.Inventory.Grpc;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
