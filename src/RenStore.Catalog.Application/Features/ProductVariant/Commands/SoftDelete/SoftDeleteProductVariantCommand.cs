namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDelete;

public sealed record SoftDeleteProductVariantCommand(Guid VariantId) : IRequest;