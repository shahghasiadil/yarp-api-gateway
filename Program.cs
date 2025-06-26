using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://your-auth-server/";
        options.Audience = "your-api-audience";
        options.RequireHttpsMetadata = true;
    });

// Add global authorization policy
builder.Services.AddAuthorization();

// Add rate limiting (fixed window, per IP)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 600; // 600 requests per minute
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add response compression
builder.Services.AddResponseCompression();

var app = builder.Build();

// Use forwarded headers for real client IP
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Enforce HTTPS
//app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Enable response compression
app.UseResponseCompression();

// Enable rate limiting
app.UseRateLimiter();

// Enable authentication/authorizat
app.UseAuthentication();
app.UseAuthorization();

// Minimal
app.MapGet("/", () => Results.Ok(new { message = "YARP API Gateway is working!" }));

// Map reverse proxy routes
app.MapReverseProxy();

app.Run();
