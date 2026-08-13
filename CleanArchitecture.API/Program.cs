using System.Text;
using CleanArchitecture.API.Middleware;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));

// ── OpenAPI / Scalar ──────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

// ── Application layer (AutoMapper + MediatR + Pipeline + FluentValidation) ───
builder.Services.AddApplication();

// ── Infrastructure layer (EF Core + Repositories + JWT services) ─────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("JwtSettings");

// Fail fast: surface a clear error if the secret was never configured.
// In development: run  dotnet user-secrets set "JwtSettings:SecretKey" "..."
// In production:  set  JWTSETTINGS__SECRETKEY  environment variable.
var jwtSecretKey = jwtSection["SecretKey"];
if (string.IsNullOrWhiteSpace(jwtSecretKey))
    throw new InvalidOperationException(
        "JwtSettings:SecretKey is not configured. " +
        "Use 'dotnet user-secrets set \"JwtSettings:SecretKey\" \"<key>\"' " +
        "in development or set the JWTSETTINGS__SECRETKEY environment variable in production.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew                = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ── Global error handling ─────────────────────────────────────────────────────
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// Auto-apply pending EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options
        .WithTitle("Clean Architecture API")
        .WithTheme(ScalarTheme.Purple)
        // Scalar Bearer auth setup:
        // 1. POST /api/auth/login → copy token from the response.
        // 2. In Scalar click the lock icon → paste the token.
        .AddPreferredSecuritySchemes("Bearer")
        .AddHttpAuthentication("Bearer", bearer =>
        {
            bearer.Token = string.Empty;  // filled in by the user after login
        }));
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

// IMPORTANT: UseAuthentication MUST be before UseAuthorization.
// Authentication identifies who the caller is (reads + validates JWT).
// Authorization then decides what the identified caller may do.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
