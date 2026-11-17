using SocialMediaManager.Domain.Enums;

namespace SocialMediaManager.Application.DTOs.Campaigns;

// الداتا اللي جاية من الشاشة عشان نعمل حملة جديدة
public class CreateCampaignDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ContentTone Tone { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
