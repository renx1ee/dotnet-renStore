namespace RenStore.Delivery.Application.Enums;

public enum RussianPostDeliveryStatus
{
    Unknown,
    Accepted,          // Принято
    Departed,          // Отправлено
    ArrivedAtSorting,  // Прибыло в сортировочный центр
    Sorted,            // Отсортировано
    EnRouteToPickup,   // Выслано в отделение
    ReadyForPickup,    // Прибыло в отделение, ожидает получателя
    Delivered,         // Вручено
    Returned,          // Возвращено
    Lost               // Утрачено
}