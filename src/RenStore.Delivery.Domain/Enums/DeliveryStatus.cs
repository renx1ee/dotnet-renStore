namespace RenStore.Delivery.Domain.Enums;

/// <summary>
/// Represents the current status of a delivery during its lifecycle.
/// </summary>
public enum DeliveryStatus
{
    /// <summary>
    /// Order has been place.
    /// Размещен.
    /// </summary>
    Placed = 0,

    /// <summary>
    /// Order is being assembled by the seller.
    /// На сборке у продавца.
    /// </summary>
    AssemblingBySeller = 1,
    
    SentToSortingCenter = 2, 

    /// <summary>
    /// Order is on the way to the sorting center.
    /// В пути к сортировочному центру.
    /// </summary>
    EnRouteToSortingCenter = 2,

    /// <summary>
    /// Order is arrived at sorting center.
    /// В пути.
    /// </summary>
    ArrivedAtSortingCenter = 3,

    /// <summary>
    /// Order is on the way to the pickup point.
    /// В пути к пункту выдачи.
    /// </summary>
    EnRouteToPickupPoint = 4,

    /// <summary>
    /// Order has arrived and is awaiting receipt by the customer.
    /// Ожидает получения.
    /// </summary>
    AwaitingPickup = 5,
    
    /// <summary>
    /// Order has been successfully delivered to the recipient.
    /// Доставлен.
    /// </summary>
    Delivered = 6,
    
    /// <summary>
    /// Order has been sorted.
    /// Отсортирован.
    /// </summary>
    Sorted = 7,
    
    /// <summary>
    /// Order has been deleted.
    /// </summary>
    IsDeleted = 8,
    
    /// <summary>
    /// Delivery is temporarily delayed.
    /// Отложенный.
    /// </summary>
    Delayed = 9,
    
    /// <summary>
    /// Order has been returned to the sender.
    /// Возвращен.
    /// </summary>
    Returned = 10
}