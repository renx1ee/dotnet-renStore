using RenStore.Order.Domain.Aggregates.Order.Events;
using RenStore.Order.Domain.Aggregates.Order.Rules;
using RenStore.Order.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Order.Domain.Aggregates.Order;

/// <summary>
/// Represents a customer order with full lifecycle management and per-item cancellation/refund support.
/// </summary>
public sealed class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();

    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identifier of the customer who placed the order.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Current lifecycle status of the order.
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Delivery address provided at order placement.
    /// </summary>
    public string ShippingAddress { get; private set; }

    /// <summary>
    /// Shipping carrier tracking number, set when order is shipped.
    /// </summary>
    public string? TrackingNumber { get; private set; }

    /// <summary>
    /// Reason for cancellation or refund, if applicable.
    /// </summary>
    public string? CancellationReason { get; private set; }

    /// <summary>
    /// Date when the order was placed.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Date of the last status change.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// All line items, including cancelled and refunded ones.
    /// </summary>
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Only active (non-cancelled, non-refunded) line items.
    /// </summary>
    public IReadOnlyCollection<OrderItem> ActiveItems =>
        _items.Where(x => x.IsActive)
              .ToList()
              .AsReadOnly();
    
    /// <summary>
    /// Total cost of all active items in the order.
    /// </summary>
    public decimal TotalAmount =>
        _items.Where(x => x.IsActive).Sum(x => x.TotalPrice);

    private Order() { }

    public static Order Create(
        DateTimeOffset now,
        Guid customerId,
        string shippingAddress)
    {
        OrderRules.ValidateCustomerId(customerId);
        var trimmedAddress = OrderRules.ValidateAndTrimShippingAddress(shippingAddress);

        var orderId = Guid.NewGuid();
        var order = new Order();

        order.Raise(new OrderCreatedEvent(
            EventId: Guid.NewGuid(),
            OrderId: orderId,
            CustomerId: customerId,
            ShippingAddress: trimmedAddress,
            OccurredAt: now));

        return order;
    }

    public Guid AddItem(
        DateTimeOffset now,
        Guid variantId,
        Guid sizeId,
        int quantity,
        decimal priceAmount,
        Currency currency,
        string productNameSnapshot)
    {
        EnsureInEditableState();

        OrderItemRules.ValidateVariantId(variantId);
        OrderItemRules.ValidateSizeId(sizeId);
        OrderItemRules.ValidateQuantity(quantity);
        OrderItemRules.ValidatePrice(priceAmount);
        var trimmedName = OrderItemRules.ValidateAndTrimProductNameSnapshot(productNameSnapshot);

        // Same variant+size combination should not appear twice as separate active items
        if (_items.Any(x => 
                x.IsActive && 
                x.VariantId == variantId && 
                x.SizeId == sizeId))
        {
            throw new DomainException(
                "An active item with the same variant and size already exists. Change its quantity instead.");
        }

        var itemId = Guid.NewGuid();

        Raise(new OrderItemAddedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OrderItemId: itemId,
            VariantId: variantId,
            SizeId: sizeId,
            Quantity: quantity,
            PriceAmount: priceAmount,
            Currency: currency,
            ProductNameSnapshot: trimmedName,
            OccurredAt: now));

        return itemId;
    }

    public void RemoveItem(
        DateTimeOffset now,
        Guid orderItemId)
    {
        EnsureInEditableState();

        var item = GetItem(orderItemId);
        EnsureItemIsActive(item);

        Raise(new OrderItemRemovedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OrderItemId: orderItemId,
            OccurredAt: now));
    }

    public void ChangeItemQuantity(
        DateTimeOffset now,
        Guid orderItemId,
        int quantity)
    {
        EnsureInEditableState();

        var item = GetItem(orderItemId);
        EnsureItemIsActive(item);

        OrderItemRules.ValidateQuantity(quantity);

        if (item.Quantity == quantity) return;

        Raise(new OrderItemQuantityChangedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OrderItemId: orderItemId,
            Quantity: quantity,
            OccurredAt: now));
    }

    public void ChangeShippingAddress(
        DateTimeOffset now,
        string shippingAddress)
    {
        EnsureInEditableState();

        var trimmedAddress = OrderRules.ValidateAndTrimShippingAddress(shippingAddress);

        if (trimmedAddress == ShippingAddress) return;

        Raise(new OrderShippingAddressChangedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            ShippingAddress: trimmedAddress,
            OccurredAt: now));
    }

    public void Confirm(DateTimeOffset now)
    {
        EnsureNotCancelledOrRefunded();

        if (Status == OrderStatus.Confirmed) return;

        if (Status != OrderStatus.Pending)
            throw new DomainException(
                $"Cannot confirm order in status '{Status}'.");

        if (!_items.Any(x => x.IsActive))
            throw new DomainException(
                "Cannot confirm an order with no active items.");

        Raise(new OrderConfirmedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OccurredAt: now));
    }

    public void MarkAsPaid(DateTimeOffset now)
    {
        EnsureNotCancelledOrRefunded();

        if (Status == OrderStatus.Paid) return;

        if (Status != OrderStatus.Confirmed)
            throw new DomainException(
                $"Cannot mark order as paid in status '{Status}'.");

        Raise(new OrderPaidEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OccurredAt: now));
    }

    public void Ship(DateTimeOffset now, string trackingNumber)
    {
        EnsureNotCancelledOrRefunded();

        if (Status == OrderStatus.Shipped) return;

        if (Status != OrderStatus.Paid)
            throw new DomainException(
                $"Cannot ship order in status '{Status}'.");

        var trimmedTracking = OrderRules.ValidateAndTrimTrackingNumber(trackingNumber);

        Raise(new OrderShippedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            TrackingNumber: trimmedTracking,
            OccurredAt: now));
    }

    public void MarkAsDelivered(DateTimeOffset now)
    {
        EnsureNotCancelledOrRefunded();

        if (Status == OrderStatus.Delivered) return;

        if (Status != OrderStatus.Shipped)
            throw new DomainException(
                $"Cannot mark order as delivered in status '{Status}'.");

        Raise(new OrderDeliveredEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OccurredAt: now));
    }

    public void Cancel(DateTimeOffset now, string reason)
    {
        if (Status == OrderStatus.Cancelled) return;

        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new DomainException(
                $"Cannot cancel order in status '{Status}'. Use refund instead.");

        var trimmedReason = OrderRules.ValidateAndTrimReason(reason);

        Raise(new OrderCancelledEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            Reason: trimmedReason,
            OccurredAt: now));
    }

    public void Refund(DateTimeOffset now, string reason)
    {
        if (Status == OrderStatus.Refunded) return;

        if (Status is not (OrderStatus.Paid or OrderStatus.Shipped or OrderStatus.Delivered))
            throw new DomainException(
                $"Cannot refund order in status '{Status}'.");

        var trimmedReason = OrderRules.ValidateAndTrimReason(reason);

        Raise(new OrderRefundedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            Reason: trimmedReason,
            OccurredAt: now));
    }

    public void CancelItem(
        DateTimeOffset now,
        Guid orderItemId,
        string reason)
    {
        EnsureNotCancelledOrRefunded();

        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new DomainException(
                $"Cannot cancel individual items in status '{Status}'.");

        var item = GetItem(orderItemId);
        EnsureItemIsActive(item);

        var trimmedReason = OrderRules.ValidateAndTrimReason(reason);

        Raise(new OrderItemCancelledEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OrderItemId: orderItemId,
            Reason: trimmedReason,
            OccurredAt: now));
    }

    public void RefundItem(
        DateTimeOffset now,
        Guid orderItemId,
        string reason)
    {
        EnsureNotCancelledOrRefunded();

        if (Status is not (OrderStatus.Paid or OrderStatus.Shipped or OrderStatus.Delivered))
            throw new DomainException(
                $"Cannot refund individual items in status '{Status}'.");

        var item = GetItem(orderItemId);
        EnsureItemIsActive(item);

        var trimmedReason = OrderRules.ValidateAndTrimReason(reason);

        Raise(new OrderItemRefundedEvent(
            EventId: Guid.NewGuid(),
            OrderId: Id,
            OrderItemId: orderItemId,
            Reason: trimmedReason,
            OccurredAt: now));
    }

    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case OrderCreatedEvent e:
                Id = e.OrderId;
                CustomerId = e.CustomerId;
                ShippingAddress = e.ShippingAddress;
                Status = OrderStatus.Pending;
                CreatedAt = e.OccurredAt;
                break;

            case OrderItemAddedEvent e:
                _items.Add(OrderItem.Create(
                    id: e.OrderItemId,
                    orderId: e.OrderId,
                    variantId: e.VariantId,
                    sizeId: e.SizeId,
                    quantity: e.Quantity,
                    priceAmount: e.PriceAmount,
                    currency: e.Currency,
                    productNameSnapshot: e.ProductNameSnapshot,
                    now: e.OccurredAt));
                UpdatedAt = e.OccurredAt;
                break;

            case OrderItemRemovedEvent e:
                var itemToRemove = _items.SingleOrDefault(x => x.Id == e.OrderItemId)
                    ?? throw new DomainException("Order item not found.");
                _items.Remove(itemToRemove);
                UpdatedAt = e.OccurredAt;
                break;

            case OrderItemQuantityChangedEvent e:
                var itemForQty = _items.SingleOrDefault(x => x.Id == e.OrderItemId)
                    ?? throw new DomainException("Order item not found.");
                itemForQty.ChangeQuantity(e.Quantity, e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;

            case OrderShippingAddressChangedEvent e:
                ShippingAddress = e.ShippingAddress;
                UpdatedAt = e.OccurredAt;
                break;

            case OrderConfirmedEvent e:
                Status = OrderStatus.Confirmed;
                UpdatedAt = e.OccurredAt;
                break;

            case OrderPaidEvent e:
                Status = OrderStatus.Paid;
                UpdatedAt = e.OccurredAt;
                break;

            case OrderShippedEvent e:
                Status = OrderStatus.Shipped;
                TrackingNumber = e.TrackingNumber;
                UpdatedAt = e.OccurredAt;
                break;

            case OrderDeliveredEvent e:
                Status = OrderStatus.Delivered;
                UpdatedAt = e.OccurredAt;
                break;

            case OrderCancelledEvent e:
                Status = OrderStatus.Cancelled;
                CancellationReason = e.Reason;
                UpdatedAt = e.OccurredAt;
                break;

            case OrderRefundedEvent e:
                Status = OrderStatus.Refunded;
                CancellationReason = e.Reason;
                UpdatedAt = e.OccurredAt;
                break;

            case OrderItemCancelledEvent e:
                var itemToCancel = _items.SingleOrDefault(x => x.Id == e.OrderItemId)
                    ?? throw new DomainException("Order item not found.");
                itemToCancel.Cancel(e.Reason, e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;

            case OrderItemRefundedEvent e:
                var itemToRefund = _items.SingleOrDefault(x => x.Id == e.OrderItemId)
                    ?? throw new DomainException("Order item not found.");
                itemToRefund.Refund(e.Reason, e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
        }
    }

    public static Order Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var order = new Order();

        foreach (var @event in history)
        {
            order.Apply(@event);
            order.Version++;
        }

        return order;
    }
    
    private void EnsureInEditableState()
    {
        if (Status is OrderStatus.Cancelled or OrderStatus.Refunded)
            throw new DomainException(
                $"Order in status '{Status}' cannot be modified.");

        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new DomainException(
                $"Order in status '{Status}' cannot be modified.");
    }

    private void EnsureNotCancelledOrRefunded()
    {
        if (Status is OrderStatus.Cancelled or OrderStatus.Refunded)
            throw new DomainException(
                $"Order is already '{Status}'.");
    }

    private void EnsureItemIsActive(OrderItem item)
    {
        if (!item.IsActive)
            throw new DomainException(
                $"Order item '{item.Id}' is not active (status: {item.Status}).");
    }

    private OrderItem GetItem(Guid orderItemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == orderItemId);

        if (item is null)
            throw new DomainException("Order item does not exist.");

        return item;
    }
}