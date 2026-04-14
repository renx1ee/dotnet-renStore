namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByUrlSlug;

public sealed record FindByUrlSlugQuery(
    string UrlSlug)
    : IRequest<ProductVariantReadModel?>;