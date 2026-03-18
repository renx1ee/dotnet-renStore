namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

public sealed record CreateProductCommand(
    Guid UserId,
    Guid SubCategoryId)
    : IRequest<Guid>;