using RenStore.Domain.Enums;
using RenStore.Microservice.Payment.Enums;

namespace RenStore.Domain.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal SubTotalPrice { get; set; }
    public decimal TaxAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? CancellationReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    public Guid PromoCodeId { get; set; }
    public PromoCodeEntity? PromoCode { get; set; }
    public DeliveryOrderEntity? DeliveryOrder { get; set; }
    public IEnumerable<PaymentEntity>? Payments { get; set; }
    public IEnumerable<OrderItemEntity>? Items { get; set; }
}