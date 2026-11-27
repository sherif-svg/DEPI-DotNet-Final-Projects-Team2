using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Application.Interfaces;
using SocialMediaManager.Domain.Entities;

namespace SocialMediaManager.Infrastructure.Persistence;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _context;

    public PostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetByIdAsync(int id)
        => await _context.Posts.FindAsync(id);

    public async Task<List<Post>> GetByCampaignIdAsync(int campaignId)
        => await _context.Posts
            .Where(p => p.CampaignId == campaignId)
            .ToListAsync();

    public async Task AddAsync(Post post)
        => await _context.Posts.AddAsync(post);

    public Task UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        return Task.CompletedTask;
    }
}