namespace RenStore.Delivery.Contracts.IntegrationEvents;

/// <summary>
/// Публикуется Order-сервисом когда заказ успешно создан и оплачен.
/// Delivery-сервис подписывается и создаёт доставку.
/// </summary>
public sealed record OrderPlacementCompletedIntegrationEvent(
    Guid    CorrelationId,
    Guid    OrderId,
    Guid    CustomerId,
    Guid    PaymentId,
    string  ShippingAddress,  // полный адрес доставки
    string  ShippingPostcode, // индекс для Почты России
    string  RecipientName,
    string  RecipientPhone,
    decimal TotalWeightGrams,
    decimal TotalValueRub);