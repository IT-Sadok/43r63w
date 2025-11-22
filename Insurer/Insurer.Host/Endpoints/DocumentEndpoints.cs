using Company.Application.Models;
using Company.Application.Models.Request;
using Company.Application.Services;
using Company.Infrastructure.FileStorage;
using Insurer.Host.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Insurer.Host.Endpoints;

public static class DocumentEndpoints
{
    public static void MapDocumentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("documents");


        group.MapPost("", CreateDocumentAsync).DisableAntiforgery();
        group.MapGet("{id}", GetDocumentAsync).DisableAntiforgery();
    }

    private static async Task<IResult> CreateDocumentAsync(
        IFormFile file,
        [FromServices] DocumentService documentService,
        [FromServices] UserContextAccessor contextAccessor,
        [FromForm] CreateDocumentModel model,
        CancellationToken cancellationToken = default)
    {
        var user = contextAccessor.GetUserContext();

        if (string.IsNullOrWhiteSpace(user.UserId))
            return Results.BadRequest("User ID is required");

        var result = await documentService.CreateDocumentAsync(file, user.UserId!, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<IResult> GetDocumentAsync(
        string id,
        [FromServices] DocumentService documentService,
        CancellationToken cancellationToken = default)
    {
        var result = await documentService.GetFileUrlAsync(id, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<IResult> DeleteDocumentAsync(
        DeleteFileRequest request,
        [FromServices] DocumentService documentService,
        CancellationToken cancellationToken = default)
    {
        var result = await documentService.DeleteFileAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.ErrorMessage);
    }
}