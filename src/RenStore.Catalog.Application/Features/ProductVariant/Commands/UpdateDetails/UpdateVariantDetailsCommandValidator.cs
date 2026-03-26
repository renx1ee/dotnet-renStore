namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateDetails;

internal sealed class UpdateVariantDetailsCommandValidator
    : AbstractValidator<UpdateVariantDetailsCommand>
{
    public UpdateVariantDetailsCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
    }
}