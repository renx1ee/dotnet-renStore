namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByArticle;

public sealed record FindVariantByArticleQuery(
    long Article)
    : IRequest<ProductVariantReadModel?>;