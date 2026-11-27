using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Application.Interfaces;
using SocialMediaManager.Domain.Entities;

namespace SocialMediaManager.Infrastructure.Persistence;

public class CampaignRepository : ICampaignRepository
{
    private readonly ApplicationDbContext _context;

    public CampaignRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Campaign?> GetByIdAsync(int id)
        => await _context.Campaigns.FindAsync(id);

    public async Task<List<Campaign>> GetAllAsync()
        => await _context.Campaigns.ToListAsync();

    public async Task AddAsync(Campaign campaign)
        => await _context.Campaigns.AddAsync(campaign);

    public Task UpdateAsync(Campaign campaign)
    {
        _context.Campaigns.Update(campaign);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Campaign campaign)
    {
        _context.Campaigns.Remove(campaign);
        return Task.CompletedTask;
    }
}