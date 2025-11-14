using User.Application.Models;
using User.Application.Services;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.Mapper;

public static class CustomerMapper
{
    public static CustomerModel ToModel(this Customer customer)
    {
        return new CustomerModel
        {
            UserId = customer.UserId,
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
        };
    }
}