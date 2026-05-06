using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Contracts.IntegrationEvents;

/// <summary>
/// Публикуется при каждом изменении статуса доставки.
/// Order-сервис и Notification-сервис могут подписаться.
/// </summary>
public sealed record DeliveryStatusChangedIntegrationEvent(
    Guid           CorrelationId,
    Guid           DeliveryOrderId,
    Guid           OrderId,
    DeliveryStatus OldStatus,
    DeliveryStatus NewStatus,
    string?        TrackingNumber,
    string?        CurrentLocation,
    DateTimeOffset OccurredAt);