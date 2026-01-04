namespace RenStore.Delivery.Domain.Enums;

/// <summary>
/// Represents available delivery tariff options.
/// </summary>
public enum DeliveryTariffType
{
    /// <summary>
    /// Standard delivery with balanced cost and delivery time.
    /// Стандартная доставка.
    /// </summary>
    Standard = 0,

    /// <summary>
    /// Expedited delivery with faster processing and shipment.
    /// Ускоренная доставка.
    /// </summary>
    Express = 1,

    /// <summary>
    /// Same-day delivery within a limited area.
    /// Доставка в день заказа.
    /// </summary>
    SameDay = 2,

    /// <summary>
    /// Economy delivery with the lowest cost and longer delivery time.
    /// Эконом-доставка.
    /// </summary>
    Economy = 3,

    /// <summary>
    /// Scheduled delivery at a specified date and time.
    /// Доставка по расписанию.
    /// </summary>
    Scheduled = 4
}