namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateAttribute;

public sealed record UpdateVariantAttributeCommand(
    Guid AttributeId,
    Guid VariantId,
    string? Key,
    string? Value)
    : IRequest,
      ISellerVariantCommand;