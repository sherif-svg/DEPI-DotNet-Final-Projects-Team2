using Hangfire.Dashboard;

namespace SocialMediaManager.Infrastructure.BackgroundJobs;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        if (httpContext.Request.Host.Host is "localhost" or "127.0.0.1")
            return true;

        return httpContext.User.Identity?.IsAuthenticated == true
            && httpContext.User.IsInRole("Admin");
    }
}