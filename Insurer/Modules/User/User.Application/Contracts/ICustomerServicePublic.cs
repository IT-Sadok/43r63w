using Shared.Results;
using User.Application.Models;
using User.Application.Services;

namespace User.Application.Contracts;

public interface ICustomerServicePublic
{
    Task<Result<bool>> CreateCustomerAsync(CreateCustomerModel model, CancellationToken cancellationToken = default);
    Task<Result<CustomerModel>> GetCustomerAsync(int Id, CancellationToken cancellationToken = default);
    Task<Result<bool>> UpdateCustomerAsync(UpdateCustomerModel model, CancellationToken cancellationToken = default);
}

internal sealed class CustomerServicePublic(CustomerService customerService) : ICustomerServicePublic
{
    public Task<Result<bool>> CreateCustomerAsync(CreateCustomerModel model,
        CancellationToken cancellationToken = default)
        => customerService.CreateCustomerAsync(model, cancellationToken);

    public Task<Result<CustomerModel>> GetCustomerAsync(int Id, CancellationToken cancellationToken = default)
        => customerService.GetCustomerAsync(Id, cancellationToken);

    public Task<Result<bool>> UpdateCustomerAsync(UpdateCustomerModel model,
        CancellationToken cancellationToken = default)
        => customerService.UpdateCustomerAsync(model, cancellationToken);
}