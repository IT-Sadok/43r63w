using Company.Application.Models;
using Company.Infrastructure.Data;
using Shared.Results;

namespace Company.Application.Services;
internal sealed class CompanyService(CompanyDbContext companyDbContext)
{
    public async Task<Result<bool>> CreateCompanyAsync(
        CreateCompanyModel model,
        CancellationToken cancellationToken = default)
    {
        var company = new Domain.Entity.Company
        {
            CompanyName = model.CompanyName,
            RegistrationNumber = model.RegistrationNumber,
            Email = model.Email,
            Phone = model.Phone,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        companyDbContext.Companies.Add(company);
        await companyDbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Success(true);
    }
}