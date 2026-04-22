using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Contracts.Events;

public sealed record VariantSizeDeletedIntegrationEvent(
    Guid VariantId,
    Guid SizeId) 
    : IIntegrationEvent;