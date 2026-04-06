using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByVariantId;

public sealed record FindFullProductPageByVariantIdQuery(
    Guid VariantId)
    : IRequest<FullProductPageDto?>;