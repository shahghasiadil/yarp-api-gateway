using Microsoft.AspNetCore.HttpOverrides;

namespace Yarp.ApiGateway.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiGatewayMiddleware(this IApplicationBuilder app)
    {
        // Use forwarded headers for real client IP
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        // Enforce HTTPS (uncomment for production)
        //app.UseHttpsRedirection();

        // Enable CORS
        app.UseCors();

        // Enable response compression
        app.UseResponseCompression();

        // Enable rate limiting
        app.UseRateLimiter();

        // Enable authentication/authorization
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
} 