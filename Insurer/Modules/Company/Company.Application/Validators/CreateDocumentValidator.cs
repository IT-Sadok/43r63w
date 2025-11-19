using Company.Application.Models;
using FluentValidation;

namespace Company.Application.Validators;

public sealed class CreateDocumentValidator : AbstractValidator<CreateDocumentModel>
{
    public CreateDocumentValidator()
    {
        RuleFor(e => e.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required.");
        
        
        RuleFor(e => e.Type)
            .IsInEnum()
            .NotNull()
            .WithMessage("Type is required.");
    }
}