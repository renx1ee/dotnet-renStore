namespace RenStore.Catalog.Contracts.Events;

public sealed record VariantSizeCreatedIntegrationEvent(
    Guid VariantId,
    Guid SizeId);