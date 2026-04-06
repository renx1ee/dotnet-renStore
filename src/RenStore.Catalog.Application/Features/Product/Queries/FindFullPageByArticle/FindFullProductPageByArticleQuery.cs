using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByArticle;

public sealed record FindFullProductPageByArticleQuery(
    long Article)
    : IRequest<FullProductPageDto?>;