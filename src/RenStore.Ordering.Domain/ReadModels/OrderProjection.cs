/*using RenStore.Ordering.Domain.Aggregates.Order.Events;
using RenStore.Ordering.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Ordering.Domain.ReadModels;

// -------------------------------------------------------------------------
// Read models
// -------------------------------------------------------------------------

// -------------------------------------------------------------------------
// Projector — rebuilds read models from the event stream
// -------------------------------------------------------------------------

/// <summary>
/// Builds <see cref="OrderReadModel"/> by replaying domain events.
/// Intended for use in read-side handlers (e.g. after handling a command,
/// or when hydrating a read model from the event store).
/// </summary>
public sealed class OrderProjector
{
    private Guid _orderId;
    private Guid _customerId;
    private OrderStatus _status;
    private string _shippingAddress = string.Empty;
    private string? _trackingNumber;
    private string? _cancellationReason;
    private DateTimeOffset _createdAt;
    private DateTimeOffset? _updatedAt;

    private readonly Dictionary<Guid, MutableOrderItem> _items = new();

    public void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case OrderCreatedEvent e:
                _orderId = e.OrderId;
                _customerId = e.CustomerId;
                _shippingAddress = e.ShippingAddress;
                _status = OrderStatus.Pending;
                _createdAt = e.OccurredAt;
                break;

            case OrderItemAddedEvent e:
                _items[e.OrderItemId] = new MutableOrderItem
                {
                    OrderItemId = e.OrderItemId,
                    OrderId = e.OrderId,
                    VariantId = e.VariantId,
                    SizeId = e.SizeId,
                    Quantity = e.Quantity,
                    PriceAmount = e.PriceAmount,
                    Currency = e.Currency,
                    ProductNameSnapshot = e.ProductNameSnapshot,
                    Status = OrderItemStatus.Active,
                    CreatedAt = e.OccurredAt
                };
                _updatedAt = e.OccurredAt;
                break;

            case OrderItemRemovedEvent e:
                _items.Remove(e.OrderItemId);
                _updatedAt = e.OccurredAt;
                break;

            case OrderItemQuantityChangedEvent e:
                if (_items.TryGetValue(e.OrderItemId, out var itemForQty))
                {
                    itemForQty.Quantity = e.Quantity;
                    itemForQty.UpdatedAt = e.OccurredAt;
                }
                _updatedAt = e.OccurredAt;
                break;

            case OrderShippingAddressChangedEvent e:
                _shippingAddress = e.ShippingAddress;
                _updatedAt = e.OccurredAt;
                break;

            case OrderConfirmedEvent e:
                _status = OrderStatus.Confirmed;
                _updatedAt = e.OccurredAt;
                break;

            case OrderPaidEvent e:
                _status = OrderStatus.Paid;
                _updatedAt = e.OccurredAt;
                break;

            case OrderShippedEvent e:
                _status = OrderStatus.Shipped;
                _trackingNumber = e.TrackingNumber;
                _updatedAt = e.OccurredAt;
                break;

            case OrderDeliveredEvent e:
                _status = OrderStatus.Delivered;
                _updatedAt = e.OccurredAt;
                break;

            case OrderCancelledEvent e:
                _status = OrderStatus.Cancelled;
                _cancellationReason = e.Reason;
                _updatedAt = e.OccurredAt;
                break;

            case OrderRefundedEvent e:
                _status = OrderStatus.Refunded;
                _cancellationReason = e.Reason;
                _updatedAt = e.OccurredAt;
                break;

            case OrderItemCancelledEvent e:
                if (_items.TryGetValue(e.OrderItemId, out var itemToCancel))
                {
                    itemToCancel.Status = OrderItemStatus.Cancelled;
                    itemToCancel.CancellationReason = e.Reason;
                    itemToCancel.UpdatedAt = e.OccurredAt;
                }
                _updatedAt = e.OccurredAt;
                break;

            case OrderItemRefundedEvent e:
                if (_items.TryGetValue(e.OrderItemId, out var itemToRefund))
                {
                    itemToRefund.Status = OrderItemStatus.Refunded;
                    itemToRefund.CancellationReason = e.Reason;
                    itemToRefund.UpdatedAt = e.OccurredAt;
                }
                _updatedAt = e.OccurredAt;
                break;
        }
    }

    public OrderReadModel BuildDetail()
    {
        var projectedItems = _items.Values
            .Select(i => new OrderItemProjection
            {
                OrderItemId = i.OrderItemId,
                OrderId = i.OrderId,
                VariantId = i.VariantId,
                SizeId = i.SizeId,
                Quantity = i.Quantity,
                PriceAmount = i.PriceAmount,
                Currency = i.Currency,
                ProductNameSnapshot = i.ProductNameSnapshot,
                Status = i.Status,
                CancellationReason = i.CancellationReason,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToList();

        var totalAmount = projectedItems
            .Where(x => x.Status == OrderItemStatus.Active)
            .Sum(x => x.TotalPrice);

        return new OrderReadModel
        {
            OrderId = _orderId,
            CustomerId = _customerId,
            Status = _status,
            ShippingAddress = _shippingAddress,
            TrackingNumber = _trackingNumber,
            CancellationReason = _cancellationReason,
            TotalAmount = totalAmount,
            CreatedAt = _createdAt,
            UpdatedAt = _updatedAt,
            Items = projectedItems
        };
    }

    public OrderSummaryProjection BuildSummary()
    {
        var activeItems = _items.Values.Where(x => x.Status == OrderItemStatus.Active).ToList();

        return new OrderSummaryProjection
        {
            OrderId = _orderId,
            CustomerId = _customerId,
            Status = _status,
            ShippingAddress = _shippingAddress,
            TrackingNumber = _trackingNumber,
            CancellationReason = _cancellationReason,
            TotalAmount = activeItems.Sum(x => x.PriceAmount * x.Quantity),
            TotalItemCount = _items.Count,
            ActiveItemCount = activeItems.Count,
            CreatedAt = _createdAt,
            UpdatedAt = _updatedAt
        };
    }

    public static OrderReadModel Project(IEnumerable<IDomainEvent> events)
    {
        var projector = new OrderProjector();
        foreach (var e in events) projector.Apply(e);
        return projector.BuildDetail();
    }

    // Private mutable helper — never exposed outside the projector
    private sealed class MutableOrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid VariantId { get; set; }
        public Guid SizeId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAmount { get; set; }
        public Currency Currency { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public OrderItemStatus Status { get; set; }
        public string? CancellationReason { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}*/