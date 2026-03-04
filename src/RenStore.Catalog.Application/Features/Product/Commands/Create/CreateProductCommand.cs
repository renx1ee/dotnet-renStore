using MediatR;

namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

public record CreateProductCommand(
    long SellerId,
    Guid SubCategoryId)
    : IRequest<Guid>;