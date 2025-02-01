using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E_commerce.Data;
using E_commerce.Repositories.Interfaces;
using E_commerce.Repositories.Implementations;
using Newtonsoft.Json;
using E_commerce.Mapp;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;
using E_commerce.Models;
using E_commerce.Repositories;
using E_commerce.Services.Interfaces;
using E_commerce.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and NewtonsoftJson for handling JSON serialization and reference loops
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

// Configure Entity Framework Core to use SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity with ApplicationUser and Role support
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>() // Make sure to use IdentityRole<int>
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole<int>>(); // Ensure roles are configured

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    string? signingKey = builder.Configuration["JWT:SigningKey"];

    if (string.IsNullOrEmpty(signingKey))
    {
        throw new InvalidOperationException("JWT SigningKey is not configured.");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
    };
});

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile)); // MappingProfile is your AutoMapper configuration

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-commerce API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the word 'Bearer' followed by a space and then your JWT token.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });

    // This ensures enums are displayed as strings (names) in Swagger UI
    c.UseInlineDefinitionsForEnums();
});


// Build the app
var app = builder.Build();

// Add the role creation logic when the app starts
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>(); // Ensure correct type here
    await EnsureRolesExistAsync(roleManager);
}

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-commerce API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Ensures the app can authenticate requests
app.UseAuthorization();  // Ensures the app can authorize users

app.MapControllers();    // Map all the controller endpoints

app.Run();

// Method to ensure roles exist
static async Task EnsureRolesExistAsync(RoleManager<IdentityRole<int>> roleManager) // Ensure correct type here
{
    // Define the roles you want to add
    string[] roles = { "Admin", "Customer" };

    foreach (var role in roles)
    {
        // Check if the role already exists
        if (!await roleManager.RoleExistsAsync(role))
        {
            // If the role doesn't exist, create it
            await roleManager.CreateAsync(new IdentityRole<int>(role)); // Ensure correct type here
        }
    }
}
