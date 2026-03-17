namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindById;

public sealed record FindVariantByIdQuery(
    Guid VariantId,
    UserRole? Role = null,
    Guid? UserId = null) 
    : IRequest<ProductVariantReadModel?>;