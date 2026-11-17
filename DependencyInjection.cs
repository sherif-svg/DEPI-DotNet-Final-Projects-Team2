using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaManager.Application.Interfaces;
using SocialMediaManager.Application.Mapping;
using SocialMediaManager.Application.Services;

namespace SocialMediaManager.Application;

public static class DependencyInjection
{
    // في Program.cs اكتب: builder.Services.AddApplication();
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<ICampaignManagementService, CampaignManagementService>();
        services.AddScoped<ISchedulingService, SchedulingService>();
        return services;
    }
}
