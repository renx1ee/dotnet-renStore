namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindById;

internal sealed class FindVariantByIdQueryValidator
    : AbstractValidator<FindVariantByIdQuery>
{
    public FindVariantByIdQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty Guid.");
    }
}