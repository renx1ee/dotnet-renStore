using System.ComponentModel.DataAnnotations;

namespace RenStore.Domain.Enums;

public enum OrderItemStatus
{
    Pending = 0,
    Processing = 1,
    PaymentConfirmed = 2,
    Shipped = 3,
    Delivered = 4,
    Completed = 5,
    Cancelled = 6,
    OnHold = 7,
    Returned = 8,
    PartiallyReturned = 9,
    DeliveryRefused = 10
}