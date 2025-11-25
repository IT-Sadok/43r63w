using Company.Application.Models.Request;
using Company.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurer.Host.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/companies").RequireAuthorization();
        group.MapPost("", CreateCompanyAsync);
        group.MapPut("{id}", UpdateCompanyAsync);
        group.MapGet("{id}", GetCompanyAsync);
    }

    private static async Task<IResult> GetCompanyAsync(
        int id,
        [FromServices] CompanyService companyService,
        CancellationToken cancellationToken = default)
    {
       var result = await companyService.GetCompanyAsync(id, cancellationToken);
       return result.IsSuccess
           ? Results.Ok(result.Value)
           : Results.BadRequest(result.Errors);
    }
    
    private static async Task<IResult> CreateCompanyAsync(
        [FromBody] CreateCompanyRequest request,
        [FromServices] CompanyService companyService,
        CancellationToken cancellationToken = default)
    {
        var result = await companyService.CreateCompanyAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> UpdateCompanyAsync(
        [FromRoute] int id,
        [FromBody] UpdateCompanyRequest request,
        [FromServices] CompanyService companyService,
        CancellationToken cancellationToken = default)
    {
        request.Id = id;
        var result = await companyService.UpdateCompanyAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}