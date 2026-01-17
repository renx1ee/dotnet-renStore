using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class OrderItemEntity
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public int Amount { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAt { get; set; }
    public OrderItemStatus Status { get; set; } = OrderItemStatus.Pending;
    public ReturnReason? ReturnReason { get; set; }
    public int ReturnedAmount { get; set; }
    public DateTime? WarrantyStartDate { get; set; } // гарантия
    public DateTime? WarrantyEndDate { get; set; }
    public Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }
    public Guid ProductId { get; set; }
    /*public ProductEntity? Product { get; set; }*/
    public IEnumerable<PaymentEntity>? Payments { get; set; }
}