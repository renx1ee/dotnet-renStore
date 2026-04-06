namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByUrlSlug;

internal sealed class FindFullProductPageByUrlSlugQueryValidator
    : AbstractValidator<FindFullProductPageByUrlSlugQuery>
{
    public FindFullProductPageByUrlSlugQueryValidator()
    {
        RuleFor(x => x.UrlSlug)
            .NotEmpty()
            .WithMessage("Url Slug cannot be empty");
    }
}