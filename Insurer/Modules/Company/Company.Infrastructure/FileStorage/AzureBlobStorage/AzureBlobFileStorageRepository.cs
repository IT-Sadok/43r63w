using Azure.Storage.Blobs;
using Company.Infrastructure.Data;
using Company.Infrastructure.FileStorage.Models;
using Microsoft.Extensions.Options;
using Minio.DataModel.Response;

namespace Company.Infrastructure.FileStorage.AzureBlobStorage;

public class AzureBlobFileStorageRepository : IFileStorageRepository
{
    private readonly CompanyDbContext _companyDbContext;
    private readonly BlobContainerClient _blobContainerClient;

    public AzureBlobFileStorageRepository(
        CompanyDbContext companyDbContext,
        IOptions<AzureBlobSettings> azureSettings,
        BlobServiceClient blobServiceClient)
    {
        _companyDbContext = companyDbContext;

        _blobContainerClient = blobServiceClient.GetBlobContainerClient(azureSettings.Value.Container);

        _blobContainerClient.CreateIfNotExists();
    }
    
    public Task<PutObjectResponse?> CreateFileAsync(
        FileUploadModel model,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetFileUrlAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFileAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}