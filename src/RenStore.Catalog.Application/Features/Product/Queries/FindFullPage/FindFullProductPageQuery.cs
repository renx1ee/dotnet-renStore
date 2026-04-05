using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPage;

public sealed record FindFullProductPageQuery(
    Guid VariantId)
    : IRequest<FullProductPageDto?>;