using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Job_Application_Management_System.Data; 
using Job_Application_Management_System.Models;
using Job_Application_Management_System.Mappings; 
using Job_Application_Management_System.Repositories.Interfaces;
using Job_Application_Management_System.Repositories.Implementations;
using Job_Application_Management_System.Services.Interfaces;
using Job_Application_Management_System.Services.Implementations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Register MVC controllers

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Connect to SQL Server using a connection string

// Configure Identity with ApplicationUser and Role support
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>() // Use EF Core for Identity
    .AddDefaultTokenProviders(); // Add default token providers for password reset, etc.

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer"; // Set the default authentication scheme to Bearer
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer(options =>
{
    // Explicitly check if JWT SigningKey exists and throw an exception if missing
    var signingKey = builder.Configuration["JWT:SigningKey"] 
                     ?? throw new InvalidOperationException("JWT SigningKey is missing.");
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true, // Ensure the token has a valid issuer
        ValidateAudience = true, // Ensure the token has a valid audience
        ValidIssuer = builder.Configuration["JWT:Issuer"], // Issuer specified in configuration
        ValidAudience = builder.Configuration["JWT:Audience"], // Audience specified in configuration
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(signingKey)) // Use the configured signing key for token validation
    };
});

// Add AutoMapper for object-to-object mapping
builder.Services.AddAutoMapper(typeof(MappingProfile)); // Register the mapping profile for AutoMapper

// Register services and repositories for dependency injection
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer(); // Enable endpoint explorer for Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Add support for JWT Bearer in Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed roles at runtime during application startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var roles = new[] { "Admin", "Recruiter", "Applicant" }; // Add roles specific to your app
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int> { Name = role }); // Create role if it doesn't exist
        }
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger in development mode
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.UseHttpsRedirection(); // Force HTTPS for all requests

app.UseStaticFiles(); // Serve static files from wwwroot folder
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware

app.MapControllers(); // Map controller endpoints

app.Run(); // Start the application
