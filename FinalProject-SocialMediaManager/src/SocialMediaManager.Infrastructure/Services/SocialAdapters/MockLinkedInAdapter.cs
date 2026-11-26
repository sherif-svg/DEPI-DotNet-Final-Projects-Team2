namespace SocialMediaManager.Infrastructure.Services.SocialAdapters;

public class MockLinkedInAdapter : ISocialAdapter
{
    public string PlatformName => "LinkedIn";

    public Task<SocialPublishResponse> PublishPostAsync(string content, string? mediaUrl = null, CancellationToken cancellationToken = default)
    {
        var mockId = $"li_{Guid.NewGuid():N}"[..15];
        return Task.FromResult(new SocialPublishResponse(true, mockId, "Published successfully to LinkedIn (Mock)."));
    }
}
