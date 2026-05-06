namespace RenStore.Delivery.Contracts.IntegrationEvents;

/// <summary>
/// Публикуется когда нужен возврат, но доставка уже в пути.
/// Notification-сервис может уведомить команду.
/// </summary>
public sealed record DeliveryReturnRequiredIntegrationEvent(
    Guid   CorrelationId,
    Guid   DeliveryOrderId,
    Guid   OrderId,
    string? TrackingNumber,
    string CurrentStatus);