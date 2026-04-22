using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Contracts.Events;

public sealed record SellerIsVerifiedChangedIntegrationEvent(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid SizeId,
    bool IsVarified)
    : IIntegrationEvent; // TODO: сделать для продукта, через ф бд сделать обновление всех проекций варианта