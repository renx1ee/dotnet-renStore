using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByUrlSlug;

public sealed record FindFullProductPageByUrlSlugQuery(
    string UrlSlug)
    : IRequest<FullProductPageDto?>;