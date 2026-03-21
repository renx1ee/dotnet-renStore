namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Restore;

internal sealed class RestoreVariantCommandValidator
    : AbstractValidator<RestoreVariantCommand>
{
    public RestoreVariantCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
    }
}