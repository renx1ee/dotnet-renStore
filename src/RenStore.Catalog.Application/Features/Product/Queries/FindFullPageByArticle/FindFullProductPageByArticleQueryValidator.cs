namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByArticle;

internal sealed class FindFullProductPageByArticleQueryValidator
    : AbstractValidator<FindFullProductPageByArticleQuery>
{
    public FindFullProductPageByArticleQueryValidator()
    {
        RuleFor(x => x.Article)
            .GreaterThan(0)
            .WithMessage("Article must be greater then 0.");
    }
}