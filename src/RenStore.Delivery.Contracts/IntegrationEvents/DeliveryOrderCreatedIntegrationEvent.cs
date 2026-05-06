namespace RenStore.Delivery.Contracts.IntegrationEvents;

/// <summary>
/// Публикуется Delivery-сервисом после создания доставки.
/// Order-сервис подписывается для обновления статуса заказа.
/// </summary>
public sealed record DeliveryOrderCreatedIntegrationEvent(
    Guid   CorrelationId,
    Guid   DeliveryOrderId,
    Guid   OrderId,
    int    TariffId);