using Auth.Application.Contracts;
using Auth.Application.Dtos;
using Auth.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Errors;
using Shared.Results;
using User.Application.Mapper;
using User.Application.Models;
using User.Domain;
using User.Domain.Entity;
using User.Domain.ValueObject;
using User.Infrastructure.Data;

namespace User.Application.Services;

internal sealed class CustomerService(
    UserDbContext userDbContext,
    IAuthServicePublic authServicePublic,
    IValidator<CreateCustomerModel> validator)
{
    public async Task<Result<bool>> CreateCustomerAsync(
        CreateCustomerModel model,
        CancellationToken cancellationToken = default)
    {
        var validate = await validator.ValidateAsync(model, cancellationToken);
        if (!validate.IsValid)
            return Result<bool>.Failure(ErrorsMessage.ValidationError);

        var entity = new Customer
        {
            UserId = model.UserId,
            FirstName = model.FirstName,
            MiddleName = model.MiddleName,
            LastName = model.LastName,
            DateOfBirth = model.DateOfBirth,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Address = new Address
            {
                City = model.Address.City,
                Country = model.Address.Country,
                Street = model.Address.Street,
                ZipCode = model.Address.ZipCode
            },
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        userDbContext.Customers.Add(entity);
        await userDbContext.SaveChangesAsync(cancellationToken);
        
        var roleModel = new AssignRoleModel
        {
            UserId = entity.UserId,
            Role = Role.Customer,
        };
        
        await authServicePublic.AssignRoleAsync(roleModel, cancellationToken);
        
        return Result<bool>.Success(true);
    }

    public async Task<Result<CustomerModel>> GetCustomerAsync(
        int Id,
        CancellationToken cancellationToken = default)
    {
        var entity = await userDbContext.Customers.FindAsync(Id, cancellationToken);

        return entity == null
            ? Result<CustomerModel>.Failure(ErrorsMessage.EntityError)
            : Result<CustomerModel>.Success(entity.ToModel());
    }

    public async Task<Result<bool>> UpdateCustomerAsync(
        UpdateCustomerModel model,
        CancellationToken cancellationToken = default)
    {
        var customer = await userDbContext.Customers.FindAsync(model.Id, cancellationToken);
        if (customer == null)
            return Result<bool>.Failure(ErrorsMessage.EntityError);
        
       if(!string.IsNullOrWhiteSpace(model.Email))
           customer.Email = model.Email;
       if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
           customer.PhoneNumber = model.PhoneNumber;
       if(model.DateOfBirth.HasValue)
           customer.DateOfBirth = model.DateOfBirth.Value;
       
        var affected = await userDbContext.SaveChangesAsync(cancellationToken);

        return affected > 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("Cannot update customer.");
    }
}