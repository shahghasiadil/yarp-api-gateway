using System.ComponentModel.DataAnnotations;

namespace Yarp.ApiGateway.Configuration;

public class GatewaySettings
{
    public const string SectionName = "Gateway";

    [Required]
    public JwtSettings Jwt { get; set; } = new();

    [Required]
    public RateLimitSettings RateLimit { get; set; } = new();

    [Required]
    public CorsSettings Cors { get; set; } = new();
}

public class JwtSettings
{
    [Required]
    [Url]
    public string Authority { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    public bool RequireHttpsMetadata { get; set; } = true;
}

public class RateLimitSettings
{
    [Range(1, 10000)]
    public int PermitLimit { get; set; } = 600;

    [Range(1, 60)]
    public int WindowMinutes { get; set; } = 1;

    [Range(0, 1000)]
    public int QueueLimit { get; set; } = 0;
}

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = ["*"];
    public string[] AllowedHeaders { get; set; } = ["*"];
    public string[] AllowedMethods { get; set; } = ["*"];
}