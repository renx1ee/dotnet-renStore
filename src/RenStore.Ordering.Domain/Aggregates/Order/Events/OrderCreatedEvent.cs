using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Order.Domain.Aggregates.Order.Events;

public sealed record OrderCreatedEvent(
    Guid EventId,
    Guid OrderId,
    Guid CustomerId,
    string ShippingAddress,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderItemAddedEvent(
    Guid EventId,
    Guid OrderId,
    Guid OrderItemId,
    Guid VariantId,
    Guid SizeId,
    int Quantity,
    decimal PriceAmount,
    Currency Currency,
    string ProductNameSnapshot,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderItemRemovedEvent(
    Guid EventId,
    Guid OrderId,
    Guid OrderItemId,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderItemQuantityChangedEvent(
    Guid EventId,
    Guid OrderId,
    Guid OrderItemId,
    int Quantity,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderConfirmedEvent(
    Guid EventId,
    Guid OrderId,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderPaidEvent(
    Guid EventId,
    Guid OrderId,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderShippedEvent(
    Guid EventId,
    Guid OrderId,
    string TrackingNumber,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderDeliveredEvent(
    Guid EventId,
    Guid OrderId,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderCancelledEvent(
    Guid EventId,
    Guid OrderId,
    string Reason,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderRefundedEvent(
    Guid EventId,
    Guid OrderId,
    string Reason,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderItemCancelledEvent(
    Guid EventId,
    Guid OrderId,
    Guid OrderItemId,
    string Reason,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderItemRefundedEvent(
    Guid EventId,
    Guid OrderId,
    Guid OrderItemId,
    string Reason,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;

public sealed record OrderShippingAddressChangedEvent(
    Guid EventId,
    Guid OrderId,
    string ShippingAddress,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;