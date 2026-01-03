namespace RenStore.Delivery.Domain.Enums;

/// <summary>
/// Represents all possible statuses of thе product tracking life cycle.  
/// </summary>
public enum DeliveryTrackingStatus
{
    /// <summary>
    /// Order has been placed but not yet processed for shippement.
    /// Оформлен.
    /// </summary>
    Placed = 0,
    
    /// <summary>
    /// Order has been shipped from the warehouse.
    /// Отправлен.
    /// </summary>
    Shipped = 1,
    
    /// <summary>
    /// Order is currently in transit between facilities.
    /// В пути.
    /// </summary>
    InTransit = 2,
    
    /// <summary>
    /// Order is out for delivery to the recipient.
    /// Доставка в пути.
    /// </summary>
    OutForDelivery = 3,
    
    /// <summary>
    /// Order has been successfully delivered to the recipient.
    /// Доставлен.
    /// </summary>
    Delivered = 4,
    
    /// <summary>
    /// Delivery is temporarily delayed.
    /// Отложенный.
    /// </summary>
    Delayed = 5,
    
    /// <summary>
    /// Order has been returned to the sender.
    /// Возвращен.
    /// </summary>
    Returned = 6
}