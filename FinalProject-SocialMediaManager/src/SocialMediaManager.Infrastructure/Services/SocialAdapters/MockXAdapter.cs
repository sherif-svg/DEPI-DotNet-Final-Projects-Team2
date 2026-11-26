namespace SocialMediaManager.Infrastructure.Services.SocialAdapters;

public class MockXAdapter : ISocialAdapter
{
    public string PlatformName => "X";

    public Task<SocialPublishResponse> PublishPostAsync(string content, string? mediaUrl = null, CancellationToken cancellationToken = default)
    {
        var mockId = $"x_{Guid.NewGuid():N}"[..14];
        return Task.FromResult(new SocialPublishResponse(true, mockId, "Published successfully to X (Mock)."));
    }
}
