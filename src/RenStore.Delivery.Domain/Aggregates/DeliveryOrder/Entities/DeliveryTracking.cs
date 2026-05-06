// Domain/Aggregates/DeliveryOrder/Entities/DeliveryTracking.cs
using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Entities;

/// <summary>
/// Запись истории трекинга. Сущность внутри агрегата DeliveryOrder.
/// Создаётся только через агрегат — не имеет самостоятельного жизненного цикла.
/// </summary>
public sealed class DeliveryTracking
{
    public Guid            Id              { get; private set; }
    public Guid            DeliveryOrderId { get; private set; }
    public DeliveryStatus  Status          { get; private set; }
    public string          Location        { get; private set; } = string.Empty;
    public string          Notes           { get; private set; } = string.Empty;
    public DateTimeOffset  OccurredAt      { get; private set; }

    private DeliveryTracking() { }

    internal static DeliveryTracking Create(
        Guid           deliveryOrderId,
        DeliveryStatus status,
        DateTimeOffset occurredAt,
        string?        location = null,
        string?        notes    = null)
    {
        if (deliveryOrderId == Guid.Empty)
            throw new DomainException("DeliveryOrderId cannot be empty.");

        return new DeliveryTracking
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = deliveryOrderId,
            Status          = status,
            OccurredAt      = occurredAt,
            Location        = location?.Trim() ?? string.Empty,
            Notes           = notes?.Trim()    ?? string.Empty
        };
    }
}