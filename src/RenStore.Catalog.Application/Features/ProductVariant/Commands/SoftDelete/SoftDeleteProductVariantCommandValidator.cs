namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDelete;

internal sealed class SoftDeleteProductVariantCommandValidator
    : AbstractValidator<SoftDeleteProductVariantCommand>
{
    public SoftDeleteProductVariantCommandValidator()
    {
        RuleFor(v => v.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}