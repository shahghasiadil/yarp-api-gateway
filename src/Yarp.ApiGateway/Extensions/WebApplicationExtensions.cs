using Yarp.ApiGateway.Middleware;

namespace Yarp.ApiGateway.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapApiGatewayEndpoints(this WebApplication app)
    {
        var environment = app.Services.GetService<IWebHostEnvironment>();
        
        // Health check endpoint
        var healthCheck = app.MapGet("/", () => Results.Ok(new
        {
            message = "YARP API Gateway is working!",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            environment = environment?.EnvironmentName ?? "Unknown"
        }));

        // Require authorization only in non-development environments
        if (environment?.IsDevelopment() != true)
        {
            healthCheck.RequireAuthorization();
        }

        // Map reverse proxy routes
        app.MapReverseProxy()
           .RequireAuthorization();

        return app;
    }

    public static WebApplication UseApiGatewayPipeline(this WebApplication app)
    {
        // Use API Gateway middleware (includes all middleware in correct order)
        app.UseApiGatewayMiddleware();

        // Add custom request logging middleware
        app.UseMiddleware<RequestLoggingMiddleware>();

        return app;
    }
}