using MediatR;

namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

public sealed record CreateProductCommand(
    long SellerId,
    Guid SubCategoryId)
    : IRequest<Guid>;