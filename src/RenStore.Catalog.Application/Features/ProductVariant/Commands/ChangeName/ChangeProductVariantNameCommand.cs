namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;

public sealed record ChangeProductVariantNameCommand(
    Guid UserId,
    Guid VariantId,
    string Name) 
    : IRequest;