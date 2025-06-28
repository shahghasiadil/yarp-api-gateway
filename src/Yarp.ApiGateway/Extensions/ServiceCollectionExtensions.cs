using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Yarp.ApiGateway.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Yarp.ApiGateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiGatewayServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Get gateway settings
        var gatewaySettings = configuration.GetSection(GatewaySettings.SectionName).Get<GatewaySettings>() ?? new GatewaySettings();
        var environment = services.BuildServiceProvider().GetService<IWebHostEnvironment>();

        // Add YARP reverse proxy
        services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));

        // Add JWT authentication with environment-specific configuration
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = gatewaySettings.Jwt.Authority;
                options.Audience = gatewaySettings.Jwt.Audience;
                options.RequireHttpsMetadata = gatewaySettings.Jwt.RequireHttpsMetadata;

                // Additional configuration for development
                if (environment?.IsDevelopment() == true)
                {
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                }
            });

        // Add rate limiting (fixed window, per IP)
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", opt =>
            {
                opt.PermitLimit = gatewaySettings.RateLimit.PermitLimit;
                opt.Window = TimeSpan.FromMinutes(gatewaySettings.RateLimit.WindowMinutes);
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = gatewaySettings.RateLimit.QueueLimit;
            });
        });

        // Add CORS
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(gatewaySettings.Cors.AllowedOrigins)
                      .WithHeaders(gatewaySettings.Cors.AllowedHeaders)
                      .WithMethods(gatewaySettings.Cors.AllowedMethods);
            });
        });

        // Add response compression
        services.AddResponseCompression();

        return services;
    }
}