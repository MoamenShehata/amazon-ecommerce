using Amazon.Customers.Application;
using Amazon.Customers.Infrastructure;
using Amazon.Customers.Infrastructure.Data;
using Amazon.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSharedJobs()
    .RegisterSharedServices(builder.Configuration)
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterInfrastructureDependencies(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration);
;

builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("CUSTOMERS_POLICY", builder =>
    {
        builder.RequireClaim("scope", "amazon.customers");
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<CustomersContext>();
var ctx2 = scope.ServiceProvider.GetRequiredService<CustomerReadContext>();
await ctx.Database.MigrateAsync();
await ctx2.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
