using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Domain.ReadModels;

public sealed class VariantReservationReadModel
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public ReservationStatus Status { get; set; }
    public ReservationCancelReason? CancelReason { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? UpdatedById { get; set; }
    public Guid StockId { get; set; }
    public string? UpdatedByRole { get; set; } = string.Empty;
    public Guid VariantId { get; set; }
    public Guid SizeId { get; set; }
    public Guid OrderId { get; set; }
}