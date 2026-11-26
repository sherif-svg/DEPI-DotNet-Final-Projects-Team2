namespace SocialMediaManager.Infrastructure.Services.Storage;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = "UseDevelopmentStorage=true";
    public string ContainerName { get; set; } = "post-media";
}
