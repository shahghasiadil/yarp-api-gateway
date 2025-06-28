# YARP API Gateway

A modern, well-organized API Gateway built with YARP (Yet Another Reverse Proxy) for .NET 8, featuring JWT authentication, rate limiting, CORS support, and response compression.

## 🏗️ Project Structure

```
Yarp.ApiGateway/
├── src/
│   └── Yarp.ApiGateway/
│       ├── Configuration/
│       │   └── GatewaySettings.cs
│       ├── Extensions/
│       │   ├── ServiceCollectionExtensions.cs
│       │   └── ApplicationBuilderExtensions.cs
│       ├── Middleware/
│       │   └── RequestLoggingMiddleware.cs
│       ├── Properties/
│       │   └── launchSettings.json
│       ├── Program.cs
│       └── Yarp.ApiGateway.csproj
├── config/
│   ├── appsettings.json
│   └── appsettings.Development.json
├── docs/
│   └── README.md (detailed documentation)
├── .gitignore
├── README.md (this file)
└── Yarp.ApiGateway.sln
```

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or any .NET-compatible IDE

### Build and Run
```bash
# Clone the repository
git clone <repository-url>
cd Yarp.ApiGateway

# Build the project
dotnet build

# Run the application
dotnet run --project src/Yarp.ApiGateway/Yarp.ApiGateway.csproj
```

The API Gateway will be available at `https://localhost:5001` (or the configured port).

## 📁 Organization

### `src/Yarp.ApiGateway/`
- **Configuration/**: Strongly-typed configuration classes
- **Extensions/**: Service collection and application builder extensions
- **Middleware/**: Custom middleware components
- **Program.cs**: Main application entry point

### `config/`
- Configuration files (`appsettings.json`, `appsettings.Development.json`)

### `docs/`
- Detailed documentation and guides

## 🔧 Features

- **Reverse Proxy**: Route requests to multiple backend services
- **JWT Authentication**: Secure your APIs with JWT token validation
- **Rate Limiting**: Protect against abuse with configurable rate limits
- **CORS Support**: Cross-origin resource sharing enabled
- **Response Compression**: Optimize response sizes
- **Request Logging**: Comprehensive request/response logging
- **Authorization Policies**: Role-based access control

## 📖 Documentation

For detailed documentation, configuration examples, and advanced usage, see the [docs/README.md](docs/README.md) file.

## 🔒 Security

- JWT token validation
- Role-based authorization
- Rate limiting per IP
- HTTPS enforcement (configurable)

## 🛠️ Development

The project follows clean architecture principles with:
- Separation of concerns
- Dependency injection
- Extension methods for clean configuration
- Strongly-typed settings

## 📄 License

[Add your license information here]

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request 