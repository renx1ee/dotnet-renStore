using RenStore.Delivery.Domain.Entities;
using RenStore.Domain.Enums;
using RenStore.Microservice.Payment.Enums;

namespace RenStore.Domain.Entities;

public class OrderEntity
{
    private readonly List<OrderItemEntity>? _items = new();
    
    public Guid Id { get; private set; }
    public decimal TotalPrice { get; set; }
    public decimal SubTotalPrice { get; set; }
    public decimal TaxAmount { get; set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public string? CancellationReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    public Guid PromoCodeId { get; set; }
    public PromoCodeEntity? PromoCode { get; set; }
    public DeliveryOrder? DeliveryOrder { get; set; }
    public IEnumerable<PaymentEntity>? Payments { get; set; }

    /*public void AddItem(ProductEntity product, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("");
        
        if (quantity <= 0)
            throw new DomainException("");
        
        _items.Add(new OrderItemEntity());
    }

    public void Place()
    {
        if (Status != OrderStatus.Delivered)
            throw new DomainException("");
        
        if (Status != OrderStatus.Delivered)
            throw new DomainException("");

        Status = OrderStatus.Delivered;
    }

    public void RecalculateTotalPrice()
    {
    }*/
}