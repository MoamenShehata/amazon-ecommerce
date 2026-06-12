var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(op =>
{
    op.AddPolicy("CORS_Policy", builder =>
    {
        builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        ;
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("YarbConfigurations"));

var app = builder.Build();

app.UseCors("CORS_Policy");
app.MapReverseProxy();

app.Run();