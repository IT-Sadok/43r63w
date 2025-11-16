using Company.Application.Models;
using Company.Infrastructure.Data;
using FluentValidation;
using Shared.Errors;
using Shared.Results;

namespace Company.Application.Services;

internal sealed class DocumentService(
    CompanyDbContext companyDbContext,
    IValidator<CreateDocumentModel> validator)
{
    public async Task<Result<bool>> CreateDocumentAsync(CreateDocumentModel model,
        CancellationToken cancellationToken = default)
    {
        var validate = await validator.ValidateAsync(model, cancellationToken);
        if (!validate.IsValid)
            return Result<bool>.Failure(ErrorsMessage.ValidationError,
                validate.Errors.ToDictionary(key => key.PropertyName, val => val.ErrorMessage));

        var entity = new Domain.Entity.Document
        {
            CompanyId = model.CompanyId,
            Name = model.Name,
            Type = model.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        companyDbContext.Documents.Add(entity);
        await companyDbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}