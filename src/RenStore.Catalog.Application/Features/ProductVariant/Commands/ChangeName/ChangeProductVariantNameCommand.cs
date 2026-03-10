using MediatR;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;

public sealed record ChangeProductVariantNameCommand(
    Guid VariantId,
    string Name) 
    : IRequest;