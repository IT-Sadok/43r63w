using FluentValidation;
using Policy.Application.Dtos;

namespace Policy.Application.Validators;

public class CreatePolicyValidator : AbstractValidator<CreatePolicyModel>
{
    public CreatePolicyValidator()
    {
        RuleFor(e => e.PolicyNumber)
            .MaximumLength(50)
            .WithMessage("Maximum length of 50 characters");
        
        RuleFor(e => e.PolicyType)
            .IsInEnum()
            .WithMessage("Policy type is not valid");

        RuleFor(x => x.PremiumAmount)
            .NotEqual(0)
            .WithMessage("Premium amount must be greater than 0");

        RuleFor(x => x.CoverageAmount)
            .GreaterThanOrEqualTo(0);


        RuleFor(e => e.UserPaymentsModel).ChildRules(cr =>
            cr.RuleFor(a => a.Amount).GreaterThanOrEqualTo(1).WithMessage("Amount must be greater than or equal to 1"));
    }
}