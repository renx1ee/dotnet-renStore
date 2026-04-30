namespace RenStore.Order.Application.Saga;

public sealed class PlaceOrderSagaState : SagaStateMachineInstance
{
    public Guid   CorrelationId { get; set; }
    public string CurrentState  { get; set; } = null!;

    // Входные данные
    public Guid CustomerId { get; set; }
    public Guid VariantId  { get; set; }
    public Guid SizeId     { get; set; }
    public int  Quantity   { get; set; }

    // Флаги параллельных запросов
    public bool SnapshotReceived { get; set; }
    public bool AddressReceived  { get; set; }

    // Данные от сервисов
    public decimal? PriceAmount         { get; set; }
    public string?  Currency            { get; set; }
    public string?  ProductNameSnapshot { get; set; }
    public string?  ShippingAddress     { get; set; }

    // Результат
    public Guid?   OrderId       { get; set; }
    public string? FailureReason { get; set; }

    // Токены таймаутов
    public Guid? ExternalDataTimeoutId { get; set; }
    public Guid? PaymentTimeoutId      { get; set; }
}