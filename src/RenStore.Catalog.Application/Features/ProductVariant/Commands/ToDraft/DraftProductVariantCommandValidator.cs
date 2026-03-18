namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ToDraft;

internal sealed class DraftProductVariantCommandValidator
    : AbstractValidator<DraftProductVariantCommand>
{
    public DraftProductVariantCommandValidator()
    {
        RuleFor(v => v.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}