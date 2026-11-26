namespace SocialMediaManager.Infrastructure.Services.SocialAdapters;

public record SocialPublishResponse(bool IsSuccess, string PlatformPostId, string Message);

public interface ISocialAdapter
{
    string PlatformName { get; }
    Task<SocialPublishResponse> PublishPostAsync(string content, string? mediaUrl = null, CancellationToken cancellationToken = default);
}
