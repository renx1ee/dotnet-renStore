using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Application.ReadModels;

public sealed record VariantReservationDto
(
    Guid                     Id,
    int                      Quantity,
    ReservationStatus        Status,
    ReservationCancelReason? CancelReason,
    DateTimeOffset           CreatedAt,
    DateTimeOffset?          UpdatedAt,
    DateTimeOffset           ExpiresAt,
    DateTimeOffset?          DeletedAt,
    Guid                     VariantId,
    Guid                     SizeId,
    Guid                     OrderId
);