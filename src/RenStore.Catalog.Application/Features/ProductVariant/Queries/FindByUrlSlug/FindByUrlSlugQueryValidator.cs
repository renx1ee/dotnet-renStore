using RenStore.Catalog.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByUrlSlug;

internal sealed class FindByUrlSlugQueryValidator
    : AbstractValidator<FindByUrlSlugQuery>
{
    public FindByUrlSlugQueryValidator()
    {
        RuleFor(x => x.UrlSlug)
            .NotEmpty()
            .MaximumLength(CatalogConstants.ProductVariant.MaxUrlLength)
            .MinimumLength(CatalogConstants.ProductVariant.MinUrlLength)
            .WithMessage(
                "Url cannot must be between " +
                $"{CatalogConstants.ProductVariant.MaxUrlLength} and " +
                $"{CatalogConstants.ProductVariant.MinUrlLength}.");
    }
}