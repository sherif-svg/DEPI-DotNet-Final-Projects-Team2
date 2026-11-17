namespace SocialMediaManager.Application.Interfaces;

// المطور 4 بينفذها بـ Hangfire
public interface IJobScheduler
{
    // يخلي Hangfire ينادي على PublishPostAsync في الوقت المحدد
    void SchedulePublish(int postId, DateTime runAt);
}
