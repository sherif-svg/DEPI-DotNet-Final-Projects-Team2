using SocialMediaManager.Domain.Enums;

namespace SocialMediaManager.Application.DTOs.Posts;

public class SchedulePostDto
{
    public int CampaignId { get; set; }
    public string Content { get; set; } = string.Empty;
    public PlatformType Platform { get; set; }
    public DateTime ScheduledAt { get; set; }
}
