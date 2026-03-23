namespace RenStore.Catalog.Application.Features.Product.Queries.FindById;

public sealed record FindProductByIdQuery(
    Guid ProductId) 
    : IRequest<ProductReadModel?>;