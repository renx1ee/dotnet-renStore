using FluentValidation;

namespace RenStore.Application.Features.Seller.Command.Create;

public class CreateSellerCommandValidator : AbstractValidator<CreateSellerCommand>
{
    public CreateSellerCommandValidator()
    {
        RuleFor(seller => seller.Name)
            .MinimumLength(3)
            .MaximumLength(30)
            .NotEmpty()
            .NotNull()
            .WithMessage("");
        
        RuleFor(seller => seller.Description)
            .MaximumLength(10)
            .MaximumLength(500)
            .NotEmpty()
            .NotNull()
            .WithMessage("");
        
        RuleFor(seller => seller.ImagePath)
            .MaximumLength(500)
            .NotEmpty()
            .NotNull()
            .WithMessage("");
        
        RuleFor(seller => seller.ImageFooterPath)
            .MaximumLength(500)
            .NotEmpty()
            .NotNull()
            .WithMessage("");
    }
}