namespace RenStore.Inventory.Contracts.Events;

public sealed record CreateReservationIntegrationEvent(
    int Quantity,
    Guid VariantId,
    Guid SizeId,
    Guid OrderId);