using Company.Application.Models;
using Company.Infrastructure.Data;
using Company.Infrastructure.FileStorage;
using Company.Infrastructure.FileStorage.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Errors;
using Shared.Results;

namespace Company.Application.Services;

internal sealed class DocumentService(
    CompanyDbContext companyDbContext,
    IFileStorageRepository fileStorageRepository,
    IValidator<CreateDocumentModel> validator)
{
    public async Task<Result<bool>> CreateDocumentAsync(CreateDocumentModel model,
        CancellationToken cancellationToken = default)
    {
        var validate = await validator.ValidateAsync(model, cancellationToken);
        if (!validate.IsValid)
            return Result<bool>.Failure(ErrorsMessage.ValidationError,
                validate.Errors.ToDictionary(key => key.PropertyName, val => val.ErrorMessage));

        var company = await companyDbContext.Companies
            .Select(e => new { e.Id, e.CompanyName })
            .FirstOrDefaultAsync(e => e.Id == model.CompanyId, cancellationToken);

        if (company == null)
            return Result<bool>.Failure(ErrorsMessage.EntityError);

        var createFileModel = new CreateFileModel
        {
            ObjectKey = $"/documents/{Guid.NewGuid()}/{model.Name}",
            CompanyName = company?.CompanyName ?? null,
            Type = model.Type.ToString(),
        };

        var fileResponse = await fileStorageRepository.CreateFileAsync(createFileModel, cancellationToken);
        if (!fileResponse.IsSuccess)
            return Result<bool>.Failure(ErrorsMessage.ErrorWhileUploadFile);

        var entity = new Domain.Entity.Document
        {
            CompanyId = model.CompanyId,
            Name = model.Name,
            Type = model.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ObjectName = fileResponse.Value!.ObjectName,
        };

        companyDbContext.Documents.Add(entity);
        await companyDbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}