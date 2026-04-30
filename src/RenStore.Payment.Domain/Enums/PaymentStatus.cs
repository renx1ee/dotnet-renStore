namespace RenStore.Payment.Domain.Enums;

public enum PaymentStatus
{
    Pending,           // создан, ожидает инициации
    Authorized,        // деньги заморожены у провайдера
    Captured,          // деньги списаны
    Failed,            // ошибка на стороне провайдера
    Cancelled,         // отменён до списания
    Refunded,          // полный возврат
    PartiallyRefunded, // частичный возврат
    Expired            // истёк срок ожидания оплаты
}