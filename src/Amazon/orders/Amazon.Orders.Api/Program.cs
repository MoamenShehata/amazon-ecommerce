using Amazon.Orders.Api;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Amazon Product Catalog API",
        Version = "v1",
        Description = "API for product catalog management"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Map OpenAPI document (existing)
    app.MapOpenApi();

    // Serve Swagger JSON and UI (dashboard) at /swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Amazon Product Catalog API v1");
        options.RoutePrefix = "swagger"; // UI available at /swagger
    });
}

app.UseCors("CORS_Policy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
