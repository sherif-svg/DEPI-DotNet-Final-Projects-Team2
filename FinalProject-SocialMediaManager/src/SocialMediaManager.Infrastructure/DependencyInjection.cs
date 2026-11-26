using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaManager.Infrastructure.Identity;
using SocialMediaManager.Infrastructure.Persistence;
using SocialMediaManager.Infrastructure.Services.SocialAdapters;
using SocialMediaManager.Infrastructure.Security;
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

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString));

        services.AddHangfireServer();
        services.AddDataProtection();
        services.Configure<OAuthSettings>(configuration.GetSection("OAuth"));
        services.AddSingleton<ITokenProtector, TokenProtector>();
        services.AddScoped<ISocialAdapter, MockMetaAdapter>();
        services.AddScoped<ISocialAdapter, MockXAdapter>();
        services.AddScoped<ISocialAdapter, MockLinkedInAdapter>();
        return services;
    }
}