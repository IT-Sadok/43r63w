using System.Net;
using Company.Application.Models.Request;
using Company.Application.Models.Responses;
using Company.Infrastructure.Data;
using Company.Infrastructure.FileStorage;
using Company.Infrastructure.FileStorage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Errors;
using Shared.Results;

namespace Company.Application.Services;

public sealed class DocumentService(
    CompanyDbContext companyDbContext,
    IFileStorageRepository fileStorageRepository,
    IOptions<MinioSettings> options)
{
    private readonly MinioSettings _minioSettings = options.Value;

    public async Task<Result<CreateDocumentResponse>> CreateDocumentAsync(
        IFormFile file,
        string userId,
        CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream();

        await file.CopyToAsync(ms, cancellationToken);
        var fileSize = Math.Round(file.Length / (1024.0 * 1024.0), 2);

        if (fileSize > _minioSettings.MaxFileSize)
            return Result<CreateDocumentResponse>.Failure("Max file [5MB] size exceeded");

        var fileMinioModel = new FileUploadModel
        {
            ObjectKey = $"/documents/{Guid.NewGuid()}/{file.FileName}",
            Content = ms.ToArray(),
            Type = file.ContentType,
        };

        var fileResponse = await fileStorageRepository.CreateFileAsync(fileMinioModel, cancellationToken);
        if (fileResponse == null || fileResponse?.ResponseStatusCode != HttpStatusCode.OK)
            return Result<CreateDocumentResponse>.Failure(ErrorsMessage.ErrorWhileUploadFile);
        
        var entity = new Domain.Entity.Document
        {
            CompanyId = 1,
            UserId = Convert.ToInt32(userId),
            Name = file.FileName,
            Type = file.ContentType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ObjectName = fileResponse.ObjectName,
        };


        companyDbContext.Documents.Add(entity);
        await companyDbContext.SaveChangesAsync(cancellationToken);


        return Result<CreateDocumentResponse>.Success(new CreateDocumentResponse
        {
            Success = true,
        });
    }

    public async Task<Result<GetFileResponse>> GetFileUrlAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var fileUrl = await fileStorageRepository.GetFileUrlAsync(id, cancellationToken);
        if (string.IsNullOrEmpty(fileUrl))
            return Result<GetFileResponse>.Failure("Fail not found");

        return Result<GetFileResponse>.Success(new GetFileResponse
        {
            FileUrl = fileUrl
        });
    }

    public async Task<Result<DeleteFileResponse>> DeleteFileAsync(
        DeleteFileRequest request,
        CancellationToken cancellationToken = default)
    {
        await fileStorageRepository.DeleteFileAsync(request.ObjectKey, cancellationToken);
        return Result<DeleteFileResponse>.Success(new DeleteFileResponse
        {
            Success = true,
        });
    }
}