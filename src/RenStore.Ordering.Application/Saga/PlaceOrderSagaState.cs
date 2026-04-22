namespace RenStore.Order.Application.Saga;

public sealed class PlaceOrderSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; } // OrderId
    
    public string CurrentState { get; set; } = string.Empty;

    // Данные от пользователя (HTTP-запрос)
    public Guid CustomerId { get; set; }
    public Guid VariantId { get; set; }
    public Guid SizeId { get; set; }
    public int Quantity { get; set; }

    // Данные от Catalog сервиса
    public decimal? PriceAmount { get; set; }
    public string? Currency { get; set; }
    public string? ProductNameSnapshot { get; set; }

    // Данные от Delivery сервиса
    public string? ShippingAddress { get; set; }

    // Флаги получения ответов (ждём оба)
    public bool PriceReceived { get; set; }
    public bool AddressReceived { get; set; }

    // Причина отказа, если один из сервисов ответил ошибкой
    public string? FailureReason { get; set; }

    // Таймаут — если сервисы не ответили вовремя
    public Guid? TimeoutTokenId { get; set; }
}