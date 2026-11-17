using SocialMediaManager.Application.DTOs.Posts;

namespace SocialMediaManager.Application.Interfaces;

// المطور 4 بينفذها (Meta / X / LinkedIn Adapters)
public interface IPostPublishingService
{
    Task<PublishResultDto> PublishAsync(PostDto post);
}
