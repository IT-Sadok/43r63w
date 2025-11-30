using Company.Application.Models;
using Company.Application.Models.Request;
using Company.Application.Models.Responses;
using Company.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Errors;
using Shared.Results;

namespace Company.Application.Services;

public sealed class CompanyService(CompanyDbContext companyDbContext)
{
    public async Task<Result<CreateCompanyResponse>> CreateCompanyAsync(
        CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var company = new Domain.Entity.Company
        {
            CompanyName = request.CompanyName,
            RegistrationNumber = request.RegistrationNumber,
            Email = request.Email,
            Phone = request.Phone,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        companyDbContext.Companies.Add(company);
        await companyDbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateCompanyResponse>.Success(new CreateCompanyResponse
        {
            CompanyId = company.Id,
            Success = true,
        });
    }

    public async Task<Result<UpdateCompanyResponse>> UpdateCompanyAsync(UpdateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var affected = await companyDbContext.Companies.Where(e => e.Id == request.Id)
            .ExecuteUpdateAsync(set =>
                set.SetProperty(e => e.CompanyName, e => request.CompanyName ?? e.CompanyName)
                    .SetProperty(e => e.Email, e => request.Email ?? e.Email)
                    .SetProperty(e => e.Phone, e => request.Phone ?? e.Phone), cancellationToken);

        if (affected == 0)
            return Result<UpdateCompanyResponse>.Failure("There is no one rows was affected");

        return Result<UpdateCompanyResponse>.Success(new UpdateCompanyResponse
        {
            Success = true,
        });
    }


    public async Task<Result<GetCompanyResponse>> GetCompanyAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var company = await companyDbContext.Companies.Select(e => new GetCompanyResponse
            {
                CompanyName = e.CompanyName,
                Email = e.Email,
                PhoneNumber = e.Phone,
            })
            .FirstOrDefaultAsync(cancellationToken);

        return company == null
            ? Result<GetCompanyResponse>.Failure(ErrorsMessage.EntityError)
            : Result<GetCompanyResponse>.Success(company);
    }
}