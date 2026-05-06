namespace RenStore.Delivery.Contracts.IntegrationEvents;

/// <summary>
/// Публикуется после регистрации в Почте России — трек-номер получен.
/// </summary>
public sealed record DeliveryOrderRegisteredIntegrationEvent(
    Guid   CorrelationId,
    Guid   DeliveryOrderId,
    Guid   OrderId,
    string TrackingNumber);