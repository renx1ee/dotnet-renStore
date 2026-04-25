namespace RenStore.Inventory.Domain.Enums;

public enum ReservationCancelReason
{
    PaymentFailed,      // оплата не прошла
    UserCancelled,      // пользователь отменил заказ
    SellerCancelled,    // продавец отменил
    Expired,            // истёк TTL
    OutOfStock,         // товар закончился (edge case при конкурентных заказах)
    FraudSuspicion,     // подозрение на мошенничество
    SystemError,        // техническая ошибка
}