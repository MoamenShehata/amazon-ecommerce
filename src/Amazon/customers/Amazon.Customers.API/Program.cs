using Amazon.Customers.Application;
using Amazon.Customers.Infrastructure;
using Amazon.SharedKernel.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSharedJobs()
    .RegisterSharedServices(builder.Configuration)
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterInfrastructureDependencies(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration);
;

builder.Services.AddCors(op =>
{
    op.AddPolicy("CORS_Policy", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:62832")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("CORS_Policy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
