using MediatR;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;

public sealed record RestoreVariantSizeCommand(
    Guid VariantId, 
    Guid SizeId) 
    : IRequest;