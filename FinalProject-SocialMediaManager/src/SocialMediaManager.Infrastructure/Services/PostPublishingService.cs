using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Application.DTOs;
using SocialMediaManager.Application.Interfaces;
using SocialMediaManager.Infrastructure.Persistence;
using SocialMediaManager.Infrastructure.Security;
using SocialMediaManager.Infrastructure.Services.SocialAdapters;

namespace SocialMediaManager.Infrastructure.Services;

public class PostPublishingService : IPostPublishingService
{
    private readonly ApplicationDbContext _context;
    private readonly IEnumerable<ISocialAdapter> _adapters;
    private readonly ITokenProtector _tokenProtector;

    public PostPublishingService(
        ApplicationDbContext context,
        IEnumerable<ISocialAdapter> adapters,
        ITokenProtector tokenProtector)
    {
        _context = context;
        _adapters = adapters;
        _tokenProtector = tokenProtector;
    }

    public async Task<PublishResultDto> PublishAsync(PostDto post)
    {
        var adapter = _adapters.FirstOrDefault(a =>
            a.PlatformName.Equals(post.Platform, StringComparison.OrdinalIgnoreCase));

        if (adapter is null)
        {
            return new PublishResultDto
            {
                Success = false,
                ErrorMessage = $"No social adapter registered for platform '{post.Platform}'."
            };
        }

        var campaign = await _context.Campaigns
            .Include(c => c.SocialAccount)
            .FirstOrDefaultAsync(c => c.Id == post.CampaignId);

        if (campaign?.SocialAccount is null)
        {
            return new PublishResultDto
            {
                Success = false,
                ErrorMessage = $"Social account not found for campaign {post.CampaignId}."
            };
        }

        _ = _tokenProtector.Unprotect(campaign.SocialAccount.AccessToken);
        var publishResponse = await adapter.PublishPostAsync(post.Content);

        return new PublishResultDto
        {
            Success = publishResponse.IsSuccess,
            ExternalPostId = publishResponse.PlatformPostId,
            ErrorMessage = publishResponse.IsSuccess ? null : publishResponse.Message
        };
    }
}