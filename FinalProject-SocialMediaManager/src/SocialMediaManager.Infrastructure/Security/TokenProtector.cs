using Microsoft.AspNetCore.DataProtection;

namespace SocialMediaManager.Infrastructure.Security;

public interface ITokenProtector
{
    string Protect(string plainToken);
    string Unprotect(string protectedToken);
}

public class TokenProtector : ITokenProtector
{
    private readonly IDataProtector _protector;

    public TokenProtector(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("SocialMediaManager.OAuthTokens.v1");
    }

    public string Protect(string plainToken) => _protector.Protect(plainToken);

    public string Unprotect(string protectedToken) => _protector.Unprotect(protectedToken);
}
