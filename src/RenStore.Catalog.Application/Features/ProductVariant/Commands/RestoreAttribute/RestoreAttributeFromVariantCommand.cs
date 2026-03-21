namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreAttribute;

public sealed record RestoreAttributeFromVariantCommand(
    Guid AttributeId,
    Guid VariantId)
    : IRequest,
      ISellerVariantCommand;