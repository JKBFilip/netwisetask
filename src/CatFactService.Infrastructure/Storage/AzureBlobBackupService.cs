using System.Text;
using Azure.Storage.Blobs.Specialized;
using CatFactService.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CatFactService.Infrastructure.Storage;

public class AzureBlobBackupService : ICloudBackupService
{
    private readonly AppendBlobClient _appendBlobClient;

    public AzureBlobBackupService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzureStorage");
        var containerName = configuration.GetSection("CloudSettings:ContainerName").Value ?? "catfacts-backup";
        var blobName = configuration.GetSection("CloudSettings:BlobName").Value ?? "cloud-catfacts.txt";
        
        _appendBlobClient = new AppendBlobClient(connectionString, containerName, blobName);
    }

    public async Task AppendFactToCloudAsync(string fact, CancellationToken cancellationToken)
    {
        await _appendBlobClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        var content = fact + Environment.NewLine;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        
        await _appendBlobClient.AppendBlockAsync(stream, cancellationToken: cancellationToken);
    }
}