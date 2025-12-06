using System.Net;
using Company.Domain.Entity;
using Company.Infrastructure.Data;
using Company.Infrastructure.FileStorage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Shared.Errors;
using Shared.Results;

namespace Company.Infrastructure.FileStorage;

public class MinioFileStorageRepository(
    CompanyDbContext dbContext,
    IOptions<MinioSettings> minioSettings,
    IMinioClient minioClient) : IFileStorageRepository
{
    private readonly MinioSettings _minioSettings = minioSettings.Value;

    public async Task<PutObjectResponse?> CreateFileAsync(
        FileUploadModel model,
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
                .WithObject(model.ObjectKey)
                .WithStreamData(new MemoryStream(model.Content))
                .WithObjectSize(model.Content.Length)
                .WithContentType(model.Type),
            cancellationToken);

        return response;
    }

    public async Task<string?> GetFileUrlAsync(
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        var fileUrl = await minioClient.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(_minioSettings.Bucket)
                .WithExpiry(3600)
                .WithObject(objectKey));

        return string.IsNullOrEmpty(fileUrl) ? null : fileUrl;
    }

    public async Task DeleteFileAsync(
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        await minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_minioSettings.Bucket)
                .WithObject(objectKey)
            , cancellationToken);
    }
}

public sealed class CreateFileResponseModel(string objectName, long size)
{
    public string ObjectName { get; set; } = objectName;

    public long Size { get; set; } = size;
}

public sealed class GetFileResponseModel
{
    public string FileUrl { get; set; } = null!;

    public bool Success { get; set; }
}