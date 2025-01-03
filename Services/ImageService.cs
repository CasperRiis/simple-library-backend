using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LibraryApi.Interfaces;

public class ImageService : IImageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string? _containerName;

    public ImageService(IConfiguration configuration)
    {
        _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
        _containerName = configuration["AzureStorage:ContainerName"];
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
        // Compute the hash of the image
        string hash;
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var hashBytes = md5.ComputeHash(imageStream);
            hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
        imageStream.Position = 0;

        string fileExtension = Path.GetExtension(fileName);
        string blobName = $"{hash}{fileExtension}";

        // Check if a blob with the same hash (name) already exists
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        // Return the URL of the existing image if it already exists
        if (await blobClient.ExistsAsync())
        {
            return blobClient.Uri.ToString();
        }

        // Upload the new image
        await blobClient.UploadAsync(imageStream, overwrite: true);
        return blobClient.Uri.ToString();
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var blobName = Path.GetFileName(uri.LocalPath);
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
    }
}