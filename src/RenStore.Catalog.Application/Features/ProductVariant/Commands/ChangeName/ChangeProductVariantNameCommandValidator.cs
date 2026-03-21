namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;

internal sealed class ChangeProductVariantNameCommandValidator
    : AbstractValidator<ChangeProductVariantNameCommand>
{
    public ChangeProductVariantNameCommandValidator()
    {
        RuleFor(v => v.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(p => p.Name)
            .MinimumLength(10)
            .MaximumLength(500)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product variant name cannot be null or empty.");
    }
}