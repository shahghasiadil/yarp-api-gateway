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
