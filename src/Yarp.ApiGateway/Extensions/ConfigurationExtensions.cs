using Yarp.ApiGateway.Configuration;

namespace Yarp.ApiGateway.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddApiGatewayConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Add strongly-typed configuration
        services.Configure<GatewaySettings>(
            configuration.GetSection(GatewaySettings.SectionName));

        // Add configuration validation
        services.AddOptions<GatewaySettings>()
            .Bind(configuration.GetSection(GatewaySettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IConfigurationBuilder AddApiGatewayConfiguration(this IConfigurationBuilder builder, IWebHostEnvironment environment)
    {
        // Get the project root directory (two levels up from the current directory)
        var projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", ".."));
        
        // Add configuration sources in order of precedence
        builder.SetBasePath(projectRoot)
               .AddJsonFile("config/appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"config/appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .AddCommandLine(Environment.GetCommandLineArgs());

        return builder;
    }
} 