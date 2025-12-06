using BloggingPlatform.Application.CommandsAndQueries.Queries.Posts;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.WebAPI.Mapper;
using BloggingPlatform.Domain.Common.Authentication;
using BloggingPlatform.Application.Repositories.Posts;
using BloggingPlatform.Application.Validators.UsersValidators;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.Persistence.Data;
using BloggingPlatform.Persistence.Repositories.Posts;
using BloggingPlatform.Persistence.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


/* Configure Serilog */
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/logfile.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

/* Register AutoMapper */
builder.Services.AddAutoMapper(typeof(MappingProfile));

/* Configure DbContext */
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/* Register MediatR */
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPostQueryHandler).Assembly));

/* Configure Identity */
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

/* Configure JWT Authentication */
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT_Param"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT_Param:Issuer"],
        ValidAudience = builder.Configuration["JWT_Param:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_Param:Key"]))
    };
});

/* Dependency Injection */
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserLoginValidator, UserLoginValidator>();
builder.Services.AddScoped<IRegisterUserValidator, RegisterUserValidator>();

/* Fluent Validation */
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Blogging Platform API",
        Version = "v1"
    });

    // Define the BearerAuth scheme that's in use
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "https://localhost:3000",
            "https://bloggingplatformfrontendwebapp-byauatf8bqf4h6ga.canadacentral-01.azurewebsites.net"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
await SeedRoles(app.Services);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blogging Platform API v1");
});

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

/* Method To Seed Roles */
async Task SeedRoles(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "User", "Admin" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
