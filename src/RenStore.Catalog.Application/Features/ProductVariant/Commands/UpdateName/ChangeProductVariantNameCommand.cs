namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateName;

public sealed record ChangeProductVariantNameCommand(
    Guid VariantId,
    string Name) 
    : IRequest,
      ISellerVariantCommand;