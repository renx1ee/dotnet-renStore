namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;

public sealed record ArchiveProductVariantCommand(Guid VariantId) : IRequest;