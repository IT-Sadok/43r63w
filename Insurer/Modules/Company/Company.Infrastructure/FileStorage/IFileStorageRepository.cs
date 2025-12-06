using Company.Infrastructure.FileStorage.Models;
using Minio.DataModel.Response;
using Shared.Results;

namespace Company.Infrastructure.FileStorage;

public interface IFileStorageRepository
{
    Task<PutObjectResponse?> CreateFileAsync(
        FileUploadModel model,
        CancellationToken cancellationToken = default);

    Task<string?> GetFileUrlAsync(
        string objectKey,
        CancellationToken cancellationToken = default);

    Task DeleteFileAsync(
        string objectKey,
        CancellationToken cancellationToken = default);
}