using System.Net.Mime;
using FluentValidation;
using User.Application.Models;

namespace User.Application.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerModel>
{
    public CreateCustomerValidator()
    {
        RuleFor(m => m.UserId)
            .NotNull();

        RuleFor(m => m.FirstName)
            .NotEmpty()
            .NotNull();

        RuleFor(m => m.LastName)
            .NotEmpty()
            .NotNull();

        RuleFor(m => m.MiddleName)
            .NotEmpty()
            .NotNull();

        RuleFor(e => e.Email)
            .EmailAddress();

        RuleFor(e => e.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .Must(IsValidPhoneNumber)
            .WithMessage("Please enter a valid phone number");

        RuleFor(e => e.DateOfBirth)
            .LessThan(DateTime.Now)
            .WithMessage("Please enter a valid date");
        
        RuleFor(address => address.Address)
            .ChildRules(address =>
                {
                     address.RuleFor(c => c.City)
                         .NotNull()
                         .NotEmpty();

                     address.RuleFor(c => c.Country)
                         .NotEmpty()
                         .NotEmpty();
                    
                });
    }
    private bool IsValidPhoneNumber(string phoneNumber) =>
        string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length > 12;
}