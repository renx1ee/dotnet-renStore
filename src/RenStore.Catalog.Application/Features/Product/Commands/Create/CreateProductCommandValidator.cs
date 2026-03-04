using FluentValidation;

namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

internal sealed class CreateProductCommandValidator 
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.SellerId)
            .GreaterThan(0)
            .WithMessage("Seller ID must be greater then 0.");
        
        RuleFor(p => p.SubCategoryId)
            .NotEmpty()
            .WithMessage("Seller ID cannot be Guid empty.");
    }
}