using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio.Exceptions;

namespace Insurer.Host.Configuration;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        
        switch (exception)
        {
            case NullReferenceException nullReferenceException:
                problemDetails.Title = "Null Reference Exception";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Instance = httpContext.Request.Path;
                problemDetails.Detail = nullReferenceException.Message;
                break;
            case SecurityTokenException securityTokenException:
                problemDetails.Title = "Token Expired";
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Instance = httpContext.Request.Path;
                problemDetails.Detail = securityTokenException.Message;
                break;
            case MinioException minioException:
                problemDetails.Title = "Minio Exception";
                problemDetails.Status = StatusCodes.Status502BadGateway;
                problemDetails.Instance = httpContext.Request.Path;
                problemDetails.Detail = minioException.Message;
                break;
            default:
                problemDetails.Title = "Internal Server Error";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Instance = httpContext.Request.Path;
                problemDetails.Detail = exception.Message;
                break;
        }
        
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}