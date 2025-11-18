using Company.Infrastructure.FileStorage.Models;
using Shared.Results;

namespace Company.Infrastructure.FileStorage;

public interface IFileStorageRepository
{
    Task<Result<CreateFileResponseModel>> CreateFileAsync(
        CreateFileModel model,
        CancellationToken cancellationToken = default);

    Task GetFileAsync(
        GetFileModel model,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteFileAsync(
        string fileName,
        CancellationToken cancellationToken = default);
}

public sealed class GetFileModel
{
    public string ObjectKey { get; set; } = null!;
}