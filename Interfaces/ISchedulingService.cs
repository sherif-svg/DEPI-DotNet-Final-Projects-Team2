using SocialMediaManager.Application.DTOs.Posts;

namespace SocialMediaManager.Application.Interfaces;

public interface ISchedulingService
{
    Task<PostDto> SchedulePostAsync(SchedulePostDto dto);
    Task<List<PostDto>> GetCampaignPostsAsync(int campaignId);
    // Hangfire بينادي عليها وقت النشر
    Task PublishPostAsync(int postId);
}
