# YARP API Gateway

A modern API Gateway built with YARP (Yet Another Reverse Proxy) for .NET 8, featuring JWT authentication, rate limiting, CORS support, and response compression.

## Features

- **Reverse Proxy**: Route requests to multiple backend services
- **JWT Authentication**: Secure your APIs with JWT token validation
- **Rate Limiting**: Protect against abuse with configurable rate limits
- **CORS Support**: Cross-origin resource sharing enabled
- **Response Compression**: Optimize response sizes
- **Forwarded Headers**: Handle real client IP addresses behind proxies
- **Authorization Policies**: Role-based access control

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or any .NET-compatible IDE

## Getting Started

### 1. Clone and Build

```bash
git clone https://github.com/shahghasiadil/yarp-api-gateway.git
cd Yarp.ApiGateway
dotnet restore
dotnet build
```

### 2. Configuration

Update the `appsettings.json` file with your configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ReverseProxy": {
    "Routes": {
      "api-route": {
        "ClusterId": "api-cluster",
        "Match": {
          "Path": "/api/{**catch-all}"
        }
      },
      "admin-route": {
        "ClusterId": "admin-cluster",
        "Match": {
          "Path": "/admin/{**catch-all}"
        },
        "AuthorizationPolicy": "AdminPolicy"
      }
    },
    "Clusters": {
      "api-cluster": {
        "Destinations": {
          "api-destination": {
            "Address": "https://your-api-service.com"
          }
        }
      },
      "admin-cluster": {
        "Destinations": {
          "admin-destination": {
            "Address": "https://your-admin-service.com"
          }
        }
      }
    }
  }
}
```

### 3. Authentication Setup

Update the JWT configuration in `Program.cs`:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://your-auth-server/";
        options.Audience = "your-api-audience";
        options.RequireHttpsMetadata = true;
    });
```

### 4. Run the Application

```bash
dotnet run
```

The API Gateway will be available at `https://localhost:5001` (or the configured port).

## Configuration Details

### Rate Limiting

The gateway implements fixed-window rate limiting:

- **Limit**: 600 requests per minute per IP
- **Window**: 1 minute
- **Queue Limit**: 0 (no queuing)

### CORS Policy

Configured to allow all origins, headers, and methods. For production, restrict this to specific domains:

```csharp
options.AddDefaultPolicy(policy =>
{
    policy.WithOrigins("https://yourdomain.com")
          .AllowAnyHeader()
          .AllowAnyMethod();
});
```

### Authorization Policies

The gateway supports multiple authorization policies:

- **Default Policy**: Requires authentication for all routes
- **AdminPolicy**: Requires "admin" role
- **UserPolicy**: Requires "user" or "admin" role
- **ReadOnlyPolicy**: Requires "read" permission

### JWT Token Requirements

Your JWT tokens should include the following claims:

```json
{
  "sub": "user123",
  "role": "admin",
  "permission": "read",
  "aud": "your-api-audience",
  "iss": "https://your-auth-server/"
}
```

## API Endpoints

### Health Check

- **GET** `/` - Returns gateway status (requires authentication)

### Proxy Routes

- All routes configured in `appsettings.json` are proxied to backend services
- Routes can have specific authorization policies applied

## Development

### Project Structure

```
Yarp.ApiGateway/
├── Program.cs                 # Main application entry point
├── appsettings.json          # Application configuration
├── appsettings.Development.json # Development-specific settings
├── Properties/
│   └── launchSettings.json   # Launch configuration
└── README.md                 # This file
```

### Adding New Routes

1. Update `appsettings.json` with new route configuration
2. Add any required authorization policies in `Program.cs`
3. Restart the application

### Custom Authorization

To add custom authorization policies:

```csharp
options.AddPolicy("CustomPolicy", policy =>
    policy.RequireAuthenticatedUser()
          .RequireClaim("custom-claim", "value"));
```

## Production Deployment

### Environment Variables

Set these environment variables for production:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://+:443
```

### HTTPS Configuration

Enable HTTPS redirection in production:

```csharp
// Uncomment in Program.cs
app.UseHttpsRedirection();
```

### Security Considerations

1. **Update JWT Authority**: Use your actual authentication server URL
2. **Restrict CORS**: Limit to specific domains
3. **Configure Rate Limits**: Adjust based on your traffic patterns
4. **Use HTTPS**: Always use HTTPS in production
5. **Monitor Logs**: Set up proper logging and monitoring

## Troubleshooting

### Common Issues

1. **Authentication Failures**: Verify JWT token format and claims
2. **Route Not Found**: Check route configuration in `appsettings.json`
3. **Rate Limiting**: Monitor rate limit headers in responses
4. **CORS Errors**: Verify CORS policy configuration

### Debug Mode

Enable detailed logging for debugging:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## Support

For issues and questions:

- Create an issue in the repository
- Check the [YARP documentation](https://microsoft.github.io/reverse-proxy/)
- Review [ASP.NET Core documentation](https://docs.microsoft.com/en-us/aspnet/core/)
