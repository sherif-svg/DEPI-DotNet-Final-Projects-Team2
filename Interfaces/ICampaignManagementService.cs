using SocialMediaManager.Application.DTOs.Campaigns;

namespace SocialMediaManager.Application.Interfaces;

// المطور 1 (الـ Controllers) بينادي على دي
public interface ICampaignManagementService
{
    Task<CampaignDto> CreateAsync(CreateCampaignDto dto);
    Task<CampaignDto> UpdateAsync(UpdateCampaignDto dto);
    Task DeleteAsync(int id);
    Task<CampaignDto> GetByIdAsync(int id);
    Task<List<CampaignDto>> GetAllAsync();
}
