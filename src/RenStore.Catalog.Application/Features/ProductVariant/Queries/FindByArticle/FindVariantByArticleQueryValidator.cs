namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByArticle;

internal sealed class FindVariantByArticleQueryValidator
    : AbstractValidator<FindVariantByArticleQuery>
{
    public FindVariantByArticleQueryValidator()
    {
        RuleFor(x => x.Article)
            .GreaterThan(0)
            .WithMessage("Article must be greater then 0.");
    }
}