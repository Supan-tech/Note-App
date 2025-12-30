using Backend.Interfaces;
using Backend.Service;
using Microsoft.EntityFrameworkCore;
using Backend.Middleware;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Logging
// =======================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// =======================
// Services
// =======================
builder.Services.AddControllers();
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

// builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .ToList();

        return new BadRequestObjectResult(new ErrorResponseDto
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = errors.Count > 0 ? "Validation failed" : "Invalid request body",
            Errors =  errors,
        });
    };
});

builder.Services.AddOptions();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});


// =======================
// Build
// =======================
var app = builder.Build();


// =======================
// Middleware
// ======================
// app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


// =======================
// Endpoints
// =======================

app.MapControllers();
app.MapGet("/app-check", () => Results.Ok("App service is working"));
app.MapGet("/db-check", async (AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { canConnect });
});

app.Run();
