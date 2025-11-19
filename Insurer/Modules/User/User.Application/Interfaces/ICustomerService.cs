using Shared.Results;
using User.Application.Models;
using User.Application.Services;

namespace User.Application.Contracts;

public interface ICustomerService
{
    Task<Result<bool>> CreateCustomerAsync(CreateCustomerModel model, CancellationToken cancellationToken = default);
    Task<Result<CustomerModel>> GetCustomerAsync(int Id, CancellationToken cancellationToken = default);
    Task<Result<bool>> UpdateCustomerAsync(UpdateCustomerModel model, CancellationToken cancellationToken = default);
}