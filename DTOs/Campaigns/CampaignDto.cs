using SocialMediaManager.Domain.Enums;

namespace SocialMediaManager.Application.DTOs.Campaigns;

// الداتا اللي بترجع للشاشة
public class CampaignDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CampaignStatus Status { get; set; }
    public ContentTone Tone { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PostsCount { get; set; }
}
