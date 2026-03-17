namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Delete;

internal sealed class DeleteVariantImageCommandValidator
    : AbstractValidator<DeleteVariantImageCommand>
{
    public DeleteVariantImageCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
    }
}