using Amazon.Notifications.Api.SignalR.Hubs;
using Amazon.Notifications.Api.SignalR.UserIdProviders;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddSignalRCore();
builder.Services.AddSingleton<IUserIdProvider, EmailUserIdProvider>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            SignatureValidator = (token, parameters) =>
            {
                var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
                return handler.ReadJsonWebToken(token);
            }
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                    context.Token = accessToken;

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();

    config.AddConsumers(typeof(Program).Assembly);

    config.UsingRabbitMq((ctxt, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]), host =>
        {
            host.Username(builder.Configuration["MessageBroker:User"]);
            host.Password(builder.Configuration["MessageBroker:Password"]);
        });
        configurator.ConfigureEndpoints(ctxt);
    });
});

builder.Services.AddCors(op =>
{
    op.AddPolicy("Cors", builder =>
    {
        builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        ;

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

app.UseCors("Cors");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Welcome to notifications service!");
app.MapHub<NotificationsHub>("/notificationHub");

app.Run();
