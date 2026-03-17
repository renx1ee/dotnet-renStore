namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindSizesByVariantId;

internal sealed class FindSizesByVariantIdQueryValidator
    : AbstractValidator<FindSizesByVariantIdQuery>
{
    public FindSizesByVariantIdQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty Guid");
    }
}