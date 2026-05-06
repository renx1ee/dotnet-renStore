namespace RenStore.Delivery.Domain.Enums;

/// <summary>
/// Represents available delivery tariff options.
/// </summary>
public enum DeliveryTariffType
{
    RussianPost,        // Почта России — основной провайдер
    RussianPostExpress  // EMS / 1-й класс — ускоренная доставка
}