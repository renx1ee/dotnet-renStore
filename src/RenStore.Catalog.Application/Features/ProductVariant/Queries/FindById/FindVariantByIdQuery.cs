namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindById;

public sealed record FindVariantByIdQuery(
    Guid VariantId) 
    : IRequest<ProductVariantReadModel?>;