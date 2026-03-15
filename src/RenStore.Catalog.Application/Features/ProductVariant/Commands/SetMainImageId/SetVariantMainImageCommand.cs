using MediatR;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SetMainImageId;

public sealed record SetVariantMainImageCommand(
    Guid VariantId,
    Guid ImageId)
    : IRequest;