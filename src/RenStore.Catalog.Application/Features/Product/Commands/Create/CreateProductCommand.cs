namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

public sealed record CreateProductCommand(
    Guid SubCategoryId)
    : IRequest<Guid>;