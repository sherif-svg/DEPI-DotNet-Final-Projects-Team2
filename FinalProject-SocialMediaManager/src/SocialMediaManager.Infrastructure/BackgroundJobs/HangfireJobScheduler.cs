using Hangfire;
using SocialMediaManager.Application.Interfaces;

namespace SocialMediaManager.Infrastructure.BackgroundJobs;

public class HangfireJobScheduler : IJobScheduler
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public HangfireJobScheduler(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public void SchedulePublish(int postId, DateTime runAt)
    {
        var utcRunAt = runAt.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(runAt, DateTimeKind.Utc)
            : runAt.ToUniversalTime();

        _backgroundJobClient.Schedule<ISchedulingService>(
            service => service.PublishPostAsync(postId),
            new DateTimeOffset(utcRunAt));
    }
}