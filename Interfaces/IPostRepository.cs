using SocialMediaManager.Domain.Entities;

namespace SocialMediaManager.Application.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(int id);
    Task<List<Post>> GetByCampaignIdAsync(int campaignId);
    Task AddAsync(Post post);
    Task UpdateAsync(Post post);
}
