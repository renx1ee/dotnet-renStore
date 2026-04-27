namespace RenStore.Order.Application.Saga;

public sealed class PlaceOrderSagaState : SagaStateMachineInstance
{
    public Guid   CorrelationId  { get; set; }
    public string CurrentState   { get; set; } = null!;

    // Входные данные
    public Guid CustomerId { get; set; }
    public Guid VariantId  { get; set; }
    public Guid SizeId     { get; set; }
    public int  Quantity   { get; set; }

    // Данные от внешних сервисов
    public decimal? PriceAmount         { get; set; }
    public string?  Currency            { get; set; }
    public string?  ProductNameSnapshot { get; set; }
    public string?  ShippingAddress     { get; set; }

    // Флаги готовности
    public bool PriceReceived   { get; set; }
    public bool AddressReceived { get; set; }

    // Результат
    public Guid?   OrderId       { get; set; }
    public string? FailureReason { get; set; }

    // Токен таймаута
    public Guid? TimeoutTokenId { get; set; }
}