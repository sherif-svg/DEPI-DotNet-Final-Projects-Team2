namespace SocialMediaManager.Infrastructure.Security;

public class OAuthSettings
{
    public bool UseMockAdapters { get; set; } = true;
    public PlatformOAuthConfig Meta { get; set; } = new();
    public PlatformOAuthConfig X { get; set; } = new();
    public PlatformOAuthConfig LinkedIn { get; set; } = new();
}

public class PlatformOAuthConfig
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}
