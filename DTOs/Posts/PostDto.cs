using SocialMediaManager.Domain.Enums;

namespace SocialMediaManager.Application.DTOs.Posts;

public class PostDto
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public string Content { get; set; } = string.Empty;
    public PlatformType Platform { get; set; }
    public DateTime ScheduledAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}
