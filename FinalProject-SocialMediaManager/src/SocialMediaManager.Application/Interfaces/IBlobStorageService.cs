namespace SocialMediaManager.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task DeleteAsync(string blobUrl, CancellationToken cancellationToken = default);
}
