namespace SocialMediaManager.Infrastructure.Services.SocialAdapters;

public class MockMetaAdapter : ISocialAdapter
{
    public string PlatformName => "Meta";

    public Task<SocialPublishResponse> PublishPostAsync(string content, string? mediaUrl = null, CancellationToken cancellationToken = default)
    {
        var mockId = $"meta_{Guid.NewGuid():N}"[..16];
        return Task.FromResult(new SocialPublishResponse(true, mockId, "Published successfully to Meta (Mock)."));
    }
}
