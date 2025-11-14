using System.Data;
using FluentValidation;
using Policy.Application.Dtos;
using Shared.Errors;

namespace Policy.Application.Validators;

public class UpdatePolicyValidator : AbstractValidator<PolicyUpdateModel>
{
    public UpdatePolicyValidator()
    {
        RuleFor(m => m.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage(ErrorsMessage.RequiredFieldError);;

        RuleFor(m => m.PolicyId)
            .GreaterThan(0)
            .WithMessage("PolicyId should be greater than 0");
    }
}