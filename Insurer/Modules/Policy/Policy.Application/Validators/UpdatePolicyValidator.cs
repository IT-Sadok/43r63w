using System.Data;
using FluentValidation;
using Policy.Application.Dtos;

namespace Policy.Application.Validators;

public class UpdatePolicyValidator : AbstractValidator<PolicyUpdateModel>
{
    public UpdatePolicyValidator()
    {
        RuleFor(m => m.UserName)
            .NotNull()
            .NotEmpty();

        RuleFor(m => m.PolicyId)
            .GreaterThan(0)
            .WithMessage("PolicyId should be greater than 0");
    }
}