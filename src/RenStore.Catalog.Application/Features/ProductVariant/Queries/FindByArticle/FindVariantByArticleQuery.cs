namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByArticle;

public sealed record FindVariantByArticleQuery(
    long Article,
    UserRole? Role = null,
    Guid? UserId = null)
    : IRequest<ProductVariantReadModel?>;