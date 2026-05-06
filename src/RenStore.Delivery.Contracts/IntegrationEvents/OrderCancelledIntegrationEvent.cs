namespace RenStore.Delivery.Contracts.IntegrationEvents;

/// <summary>
/// Публикуется Order-сервисом при отмене заказа.
/// Delivery-сервис удаляет доставку если она ещё не передана Почте.
/// </summary>
public sealed record OrderCancelledIntegrationEvent(
    Guid   CorrelationId,
    Guid   OrderId,
    string Reason);