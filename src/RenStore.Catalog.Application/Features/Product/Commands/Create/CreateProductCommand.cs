namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

public sealed record CreateProductCommand(
    Guid SellerId,
    Guid SubCategoryId)
    : IRequest<Guid>;