namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Restore;

public sealed record RestoreVariantCommand(
    Guid VariantId) 
    : IRequest,
        ISellerVariantCommand;