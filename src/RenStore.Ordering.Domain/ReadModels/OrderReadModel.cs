using RenStore.Order.Domain.Enums;

namespace RenStore.Order.Domain.ReadModels;

/// <summary>
/// Full order read model including all line items.
/// </summary>
public sealed class OrderReadModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
    public string? CancellationReason { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public IReadOnlyList<OrderItemReadModel> Items { get; set; } = [];
}