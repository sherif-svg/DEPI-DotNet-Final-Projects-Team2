using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using SocialMediaManager.Application.Interfaces;

namespace SocialMediaManager.Infrastructure.Services.Storage;

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorageService(IOptions<BlobStorageSettings> settings)
    {
        var config = settings.Value;
        var blobServiceClient = new BlobServiceClient(config.ConnectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(config.ContainerName);
    }

    public async Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        var extension = Path.GetExtension(fileName);
        var uniqueBlobName = $"{Guid.NewGuid()}{extension}";
        var blobClient = _containerClient.GetBlobClient(uniqueBlobName);

        var headers = new BlobHttpHeaders { ContentType = contentType };
        await blobClient.UploadAsync(
            fileStream,
            new BlobUploadOptions { HttpHeaders = headers },
            cancellationToken);

        return blobClient.Uri.ToString();
    }

    public async Task DeleteAsync(string blobUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(blobUrl))
            return;

        var uri = new Uri(blobUrl);
        var blobName = Path.GetFileName(uri.LocalPath);
        var blobClient = _containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}
