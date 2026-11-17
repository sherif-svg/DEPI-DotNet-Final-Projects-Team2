using SocialMediaManager.Domain.Entities;

namespace SocialMediaManager.Application.Interfaces;

// المطور 4 هو اللي هينفذها بـ EF Core
public interface ICampaignRepository
{
    Task<Campaign?> GetByIdAsync(int id);
    Task<List<Campaign>> GetAllAsync();
    Task AddAsync(Campaign campaign);
    Task UpdateAsync(Campaign campaign);
    Task DeleteAsync(Campaign campaign);
}
