using Shared.Results;
using User.Application.Models;
using User.Application.Models.Responses;
using User.Application.Services;

namespace User.Application.Contracts;

public interface ICustomerService
{
    Task<Result<CreateCustomerResponse>> CreateCustomerAsync(CreateCustomerModel model,
        CancellationToken cancellationToken = default);

    Task<Result<CustomerModel>> GetCustomerAsync(int Id, CancellationToken cancellationToken = default);

    Task<Result<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerModel model,
        CancellationToken cancellationToken = default);
}