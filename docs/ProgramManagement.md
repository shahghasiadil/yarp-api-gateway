# Program.cs Management Guide

This guide explains how to effectively manage the `Program.cs` file in your YARP API Gateway project.

## 📋 Current Program.cs Structure

```csharp
using Yarp.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure API Gateway configuration sources
builder.Configuration.AddApiGatewayConfiguration(builder.Environment);

// Add API Gateway configuration with validation
builder.Services.AddApiGatewayConfiguration(builder.Configuration);

// Add API Gateway services (includes all services: YARP, JWT, Auth, Rate Limiting, CORS, etc.)
builder.Services.AddApiGatewayServices(builder.Configuration);

var app = builder.Build();

// Configure API Gateway pipeline
app.UseApiGatewayPipeline();

// Map API Gateway endpoints
app.MapApiGatewayEndpoints();

app.Run();
```

## 🏗️ Architecture Overview

### **Extension Methods Organization**

The `Program.cs` file is kept minimal by using extension methods organized in the `Extensions/` folder:

1. **ConfigurationExtensions.cs** - Configuration setup and validation
2. **ServiceCollectionExtensions.cs** - Service registration
3. **ApplicationBuilderExtensions.cs** - Middleware configuration
4. **WebApplicationExtensions.cs** - Endpoint mapping and pipeline setup

### **Configuration Management**

```csharp
// Strongly-typed configuration with validation
builder.Services.AddApiGatewayConfiguration(builder.Configuration);
```

This includes:
- Configuration binding with validation
- Data annotations validation
- Startup validation

### **Service Registration**

```csharp
// All services registered in one place
builder.Services.AddApiGatewayServices(builder.Configuration);
```

This includes:
- YARP reverse proxy
- JWT authentication
- Authorization policies
- Rate limiting
- CORS
- Response compression

### **Middleware Pipeline**

```csharp
// All middleware configured in correct order
app.UseApiGatewayPipeline();
```

This includes:
- Forwarded headers
- CORS
- Response compression
- Rate limiting
- Authentication/Authorization
- Custom request logging

### **Endpoint Mapping**

```csharp
// All endpoints mapped in one place
app.MapApiGatewayEndpoints();
```

This includes:
- Health check endpoint
- Reverse proxy routes

## 🔧 How to Add New Features

### **1. Adding New Services**

Create or update `ServiceCollectionExtensions.cs`:

```csharp
public static IServiceCollection AddApiGatewayServices(this IServiceCollection services, IConfiguration configuration)
{
    // Existing services...
    
    // Add your new service
    services.AddYourNewService();
    
    return services;
}
```

### **2. Adding New Middleware**

Create or update `ApplicationBuilderExtensions.cs`:

```csharp
public static IApplicationBuilder UseApiGatewayMiddleware(this IApplicationBuilder app)
{
    // Existing middleware...
    
    // Add your new middleware
    app.UseYourNewMiddleware();
    
    return app;
}
```

### **3. Adding New Endpoints**

Create or update `WebApplicationExtensions.cs`:

```csharp
public static WebApplication MapApiGatewayEndpoints(this WebApplication app)
{
    // Existing endpoints...
    
    // Add your new endpoint
    app.MapGet("/your-endpoint", () => Results.Ok("Your endpoint"));
    
    return app;
}
```

### **4. Adding New Configuration**

Update `GatewaySettings.cs`:

```csharp
public class GatewaySettings
{
    // Existing settings...
    
    [Required]
    public YourNewSettings YourNew { get; set; } = new();
}

public class YourNewSettings
{
    [Required]
    public string Setting1 { get; set; } = string.Empty;
    
    [Range(1, 100)]
    public int Setting2 { get; set; } = 10;
}
```

## 🎯 Best Practices

### **1. Keep Program.cs Minimal**
- Only include high-level orchestration
- Use extension methods for implementation details
- Avoid business logic in Program.cs

### **2. Use Extension Methods**
- Group related functionality
- Make code reusable
- Improve readability

### **3. Validate Configuration**
- Use data annotations
- Validate on startup
- Provide clear error messages

### **4. Follow Middleware Order**
- Security first (HTTPS, CORS)
- Performance (compression, caching)
- Authentication/Authorization
- Business logic
- Logging

### **5. Environment-Specific Configuration**
- Use `appsettings.{Environment}.json`
- Environment variables for secrets
- Command line arguments for overrides

## 🔍 Debugging and Troubleshooting

### **Configuration Issues**

```csharp
// Add this to Program.cs for debugging
var config = builder.Configuration.GetSection("Gateway").Get<GatewaySettings>();
Console.WriteLine($"JWT Authority: {config?.Jwt.Authority}");
```

### **Middleware Order Issues**

Check the order in `ApplicationBuilderExtensions.cs`:
1. Forwarded headers
2. HTTPS redirection
3. CORS
4. Response compression
5. Rate limiting
6. Authentication
7. Authorization
8. Custom middleware

### **Service Registration Issues**

Verify services in `ServiceCollectionExtensions.cs`:
- All required services are registered
- Configuration is properly bound
- Dependencies are resolved

## 📈 Scaling Considerations

### **For Large Applications**

1. **Split by Feature**: Create separate extension methods for each feature
2. **Conditional Registration**: Use environment or configuration to conditionally register services
3. **Modular Configuration**: Split configuration into multiple files
4. **Health Checks**: Add comprehensive health checks

### **Example: Feature-Based Organization**

```csharp
// In ServiceCollectionExtensions.cs
public static IServiceCollection AddApiGatewayServices(this IServiceCollection services, IConfiguration configuration)
{
    services.AddCoreServices(configuration)
            .AddSecurityServices(configuration)
            .AddMonitoringServices(configuration)
            .AddFeatureServices(configuration);
    
    return services;
}
```

## 🚀 Performance Tips

1. **Lazy Loading**: Use lazy initialization for heavy services
2. **Configuration Caching**: Cache configuration values
3. **Middleware Optimization**: Only add necessary middleware
4. **Service Lifetime**: Use appropriate service lifetimes (Singleton, Scoped, Transient)

## 📚 Additional Resources

- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [YARP Documentation](https://microsoft.github.io/reverse-proxy/)
- [Extension Methods Best Practices](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) 