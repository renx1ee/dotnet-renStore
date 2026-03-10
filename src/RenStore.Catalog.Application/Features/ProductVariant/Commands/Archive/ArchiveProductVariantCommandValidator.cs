using FluentValidation;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;

internal sealed class ArchiveProductVariantCommandValidator
    : AbstractValidator<ArchiveProductVariantCommand>
{
    public ArchiveProductVariantCommandValidator()
    {
        RuleFor(v => v.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
    }
}