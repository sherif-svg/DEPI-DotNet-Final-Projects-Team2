using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SocialMediaManager.Application.Interfaces;
using SocialMediaManager.Infrastructure.Identity;
using SocialMediaManager.Infrastructure.Persistence;
using SocialMediaManager.Infrastructure.Security;
using SocialMediaManager.Infrastructure.Services.SocialAdapters;
using SocialMediaManager.Infrastructure.Services.Storage;

namespace SocialMediaManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var jwtSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSection);
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        var secretKey = jwtSection["SecretKey"] ?? "SuperSecretKeyForSocialMediaManagerDev2026!@#";
        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "SocialMediaManager_";
        });

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString));

        services.AddHangfireServer();
        services.AddDataProtection();

        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        services.Configure<OAuthSettings>(configuration.GetSection("OAuth"));
        services.AddSingleton<ITokenProtector, TokenProtector>();
        services.AddScoped<ISocialAdapter, MockMetaAdapter>();
        services.AddScoped<ISocialAdapter, MockXAdapter>();
        services.AddScoped<ISocialAdapter, MockLinkedInAdapter>();

        return services;
    }
}
