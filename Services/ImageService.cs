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
    {    // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new InvalidOperationException("Invalid file type.");
        }

        // Limit file size (e.g., 5MB)
        const int maxFileSize = 5 * 1024 * 1024;
        if (imageStream.Length > maxFileSize)
        {
            throw new InvalidOperationException("File size exceeds the limit.");
        }

        // Compute the hash of the image
        string hash;
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var hashBytes = md5.ComputeHash(imageStream);
            hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
        imageStream.Position = 0;

        // Check if a blob with the same hash (name) already exists
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        string blobName = $"{hash}{fileExtension}";
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