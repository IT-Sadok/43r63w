using System.Net;
using Company.Infrastructure.FileStorage.Models;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Shared.Errors;
using Shared.Results;

namespace Company.Infrastructure.FileStorage;

public class MinioFileStorageRepository(
    IOptions<MinioSettings> minioSettings,
    IMinioClient minioClient) : IFileStorageRepository
{
    private readonly MinioSettings _minioSettings = minioSettings.Value;

    public async Task<Result<CreateFileResponseModel>> CreateFileAsync(
        CreateFileModel model,
        CancellationToken cancellationToken = default)
    {
        var isBucketExists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_minioSettings.Bucket), cancellationToken);

        if (!isBucketExists)
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_minioSettings.Bucket), cancellationToken);
        
        var response = await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_minioSettings.Bucket)
                .WithFileName(model.ObjectKey)
                .WithContentType(model.Type.ToString()), cancellationToken);

        return response.ResponseStatusCode == HttpStatusCode.OK
            ? Result<CreateFileResponseModel>.Success(new CreateFileResponseModel(response.ObjectName, response.Size))
            : Result<CreateFileResponseModel>.Failure("");
    }

    public async Task GetFileAsync(
        GetFileModel model, 
        CancellationToken cancellationToken = default)
    {
        var file = await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs().WithObject(model.ObjectKey));
    }

    public Task<Result<bool>> DeleteFileAsync(
        string fileName,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public sealed class CreateFileResponseModel(string objectName, long size)
{
    public string ObjectName { get; set; } = objectName;

    public long Size { get; set; } = size;
}