namespace RenStore.Catalog.Application.Features.Product.Queries.FindById;

public sealed record FindProductByIdQuery(
    Guid ProductId,
    UserRole? Role = null,
    Guid? UserId = null) 
    : IRequest<ProductReadModel?>;