using DotNetIdentityAPI.CustomMiddleware;
using DotNetIdentityAPI.CustomPolicy;
using DotNetIdentityAPI.Models;
using DotNetIdentityAPI.Seeding;
using DotNetIdentityAPI.Services;
using DotNetIdentityShared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add Db Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// Add identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders(); ;

// Add jwt authentication
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters =
    new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudiences = builder.Configuration["AuthSettings:Audiences"].Split(","),
        ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:Key"])),
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options =>
{
    // Policies to validate token audience
    options.AddPolicy("ValidClient1AudiencePolicy", p => p.RequireClaim(JwtRegisteredClaimNames.Aud, "client1-audience"));
    options.AddPolicy("ValidClient2AudiencePolicy", p => p.RequireClaim(JwtRegisteredClaimNames.Aud, "client2-audience"));

    // Policies to validate permissions
    options.AddPolicy("CreateOrderPolicy", p => p.RequireRole("OrderCreator"));
    options.AddPolicy("CreateProductPolicy", p => p.RequireRole("ProductCreator"));

    // Custom Requirement
    //options.AddPolicy("CustomCreateOrderPolicy", p => p.Requirements.Add(new CustomRequirementAudienceAndRole("client1-audience", "OrderCreator")));
});

// Register user service
builder.Services.AddScoped<IUserService, UserService>();

// Register custom policy handler
builder.Services.AddSingleton<IAuthorizationHandler, CustomAudienceAndRoleAuthorizationHandler>();

var app = builder.Build();

// register the custom middleware in ioc to change the forbidden response
//app.UseMiddleware<CustomizeAuthorizationFailureMessageMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await Seeding.SeedDataAndApplyPendingMigrationsAsync(app.Services);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
