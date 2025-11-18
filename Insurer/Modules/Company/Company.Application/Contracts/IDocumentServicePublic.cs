using Company.Application.Models;
using Company.Application.Services;
using Shared.Results;

namespace Company.Application.Contracts;

public interface IDocumentServicePublic
{
    Task<Result<bool>> CreateDocumentAsync(
        CreateDocumentModel model,
        CancellationToken cancellationToken = default);
}

internal sealed class DocumentServicePublic(DocumentService documentService) : IDocumentServicePublic
{
    public Task<Result<bool>> CreateDocumentAsync(
        CreateDocumentModel model,
        CancellationToken cancellationToken = default)
        => documentService.CreateDocumentAsync(model, cancellationToken);
}