using SocialMediaManager.Domain.Enums;

namespace SocialMediaManager.Application.DTOs.Campaigns;

public class UpdateCampaignDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ContentTone Tone { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
