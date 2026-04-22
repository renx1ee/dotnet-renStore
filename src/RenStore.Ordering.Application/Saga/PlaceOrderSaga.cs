using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Saga.Records;

namespace RenStore.Order.Application.Saga;

public sealed class PlaceOrderSaga
    : MassTransitStateMachine<PlaceOrderSagaState>
{
    private readonly ILogger<PlaceOrderSaga> _logger;

    // States
    public State WaitingForExternalData { get; private set; } = null!;
    public State PlacingOrder           { get; private set; } = null!;
    public State Completed              { get; private set; } = null!;
    public State Failed                 { get; private set; } = null!;

    // Events
    public Event<InitiateOrderPlacement>  OrderInitiated  { get; private set; } = null!;
    public Event<VariantPriceReceived>    PriceReceived   { get; private set; } = null!;
    public Event<VariantPriceFailed>      PriceFailed     { get; private set; } = null!;
    public Event<ShippingAddressReceived> AddressReceived { get; private set; } = null!;
    public Event<ShippingAddressFailed>   AddressFailed   { get; private set; } = null!;
    public Schedule<PlaceOrderSagaState, SagaTimeout>  ResponseTimeout { get; private set; } = null!;

    public PlaceOrderSaga(
        ILogger<PlaceOrderSaga> logger)
    {
        _logger = logger;
        
        InstanceState(x => x.CurrentState);

        Event(() => OrderInitiated, 
            x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        
        Event(() => PriceReceived, 
            x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        
        Event(() => PriceFailed, 
            x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        
        Event(() => AddressReceived, 
            x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        
        Event(() => AddressFailed, 
            x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

        Schedule(
            () => ResponseTimeout, 
            x => x.TimeoutTokenId, 
            s =>
            {
                s.Delay = TimeSpan.FromSeconds(30);
                s.Received = r => r.CorrelateById(ctx => ctx.Message.CorrelationId);
            });

        ConfigureStateMachine();
    }

    private void ConfigureStateMachine()
    {
        Initially(
            When(OrderInitiated)
                .Then(ctx =>
                {
                    var msg = ctx.Message;
                    ctx.Saga.CustomerId = msg.CustomerId;
                    ctx.Saga.VariantId  = msg.VariantId;
                    ctx.Saga.SizeId     = msg.SizeId;
                    /*ctx.Saga.Quantity   = msg.Quantity;*/
                    
                    _logger.LogInformation(
                        "PlaceOrderSaga started. OrderId={OrderId}, CustomerId={CustomerId}.",
                        ctx.Saga.CorrelationId,
                        msg.CustomerId);
                })
                .PublishAsync(ctx => ctx.Init<GetVariantPriceRequest>(new GetVariantPriceRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId)))
                .PublishAsync(ctx => ctx.Init<GetShippingAddressRequest>(new GetShippingAddressRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    CustomerId:    ctx.Saga.CustomerId)))
                /*.Schedule(ResponseTimeout, ctx => ctx.Init<SagaTimeout>(new SagaTimeout(ctx.Saga.CorrelationId)))*/
                .TransitionTo(WaitingForExternalData)
            );

        During(WaitingForExternalData,

            // Цена получена успешно
            When(PriceReceived)
                .Then(ctx =>
                {
                    ctx.Saga.PriceAmount = ctx.Message.PriceAmount;
                    ctx.Saga.Currency = ctx.Message.Currency;
                    ctx.Saga.ProductNameSnapshot = ctx.Message.ProductNameSnapshot;
                    ctx.Saga.PriceReceived = true;

                    _logger.LogDebug(
                        "PlaceOrderSaga: price received. OrderId={OrderId} Price={Price}",
                        ctx.Saga.CorrelationId,
                        ctx.Message.PriceAmount);
                })
                .IfElse(
                    // Если уже получили адрес — переходим к созданию заказа
                    ctx => ctx.Saga.AddressReceived,
                    ready => ready.TransitionTo(PlacingOrder),
                    waiting => waiting.TransitionTo(WaitingForExternalData)),

            // Адрес получен успешно
            When(AddressReceived)
                .Then(ctx =>
                {
                    ctx.Saga.ShippingAddress = ctx.Message.ShippingAddress;
                    ctx.Saga.AddressReceived = true;

                    _logger.LogDebug(
                        "PlaceOrderSaga: address received. OrderId={OrderId}",
                        ctx.Saga.CorrelationId);
                })
                .IfElse(
                    // Если уже получили цену — переходим к созданию заказа
                    ctx => ctx.Saga.PriceReceived,
                    ready => ready.TransitionTo(PlacingOrder),
                    waiting => waiting.TransitionTo(WaitingForExternalData)),

            // Catalog вернул ошибку
            When(PriceFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Catalog service failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "PlaceOrderSaga: price fetch failed. OrderId={OrderId} Reason={Reason}",
                        ctx.Saga.CorrelationId,
                        ctx.Message.Reason);
                })
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason: ctx.Saga.FailureReason!))),

            When(AddressFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Delivery service failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "PlaceOrderSaga: address fetch failed. OrderId={OrderId} Reason={Reason}",
                        ctx.Saga.CorrelationId,
                        ctx.Message.Reason);
                })
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason: ctx.Saga.FailureReason!)))

            /*When(ResponseTimeout.Received)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason =
                        $"Timeout waiting for external services. " +
                        $"PriceReceived={ctx.Saga.PriceReceived} " +
                        $"AddressReceived={ctx.Saga.AddressReceived}";

                    _logger.LogError(
                        "PlaceOrderSaga: timeout. OrderId={OrderId}", ctx.Saga.CorrelationId);
                })
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))*/
        );

        WhenEnter(PlacingOrder, binder => binder
            .ThenAsync(async ctx =>
            {
                var saga = ctx.Saga;
                
                // TODO: send event
            })
            .Unschedule(ResponseTimeout)
            .TransitionTo(Completed)
            .PublishAsync(ctx => 
                ctx.Init<OrderPlacementCompleted>(new OrderPlacementCompleted(
                    CorrelationId: ctx.Saga.CorrelationId,
                    OrderId:       ctx.Saga.CorrelationId))));
        
        SetCompletedWhenFinalized();
    }
}

/*try
                    {
                        // Резолвим зависимости из контейнера MassTransit
                        var dbContext = ctx.GetPayload<OrderingDbContext>();
                        var now       = DateTimeOffset.UtcNow;

                        var currency = Enum.Parse<Currency>(saga.Currency!, ignoreCase: true);

                        // Создаём агрегат — вся бизнес-логика здесь
                        var order = Order.Create(
                            now:             now,
                            customerId:      saga.CustomerId,
                            shippingAddress: saga.ShippingAddress!);

                        order.AddItem(
                            now:                 now,
                            variantId:           saga.VariantId,
                            sizeId:              saga.SizeId,
                            quantity:            saga.Quantity,
                            priceAmount:         saga.PriceAmount!.Value,
                            currency:            currency,
                            productNameSnapshot: saga.ProductNameSnapshot!);

                        // Строим проекции
                        var events    = order.DomainEvents;
                        var projector = new OrderProjector();
                        foreach (var e in events) projector.Apply(e);

                        dbContext.OrderSummaries.Add(projector.BuildSummary());
                        dbContext.OrderDetails.Add(projector.BuildDetail());

                        // Пишем в outbox — в той же транзакции
                        OutboxWriter.Write(
                            context:     dbContext,
                            aggregateId: order.Id,
                            events:      events,
                            now:         now);

                        await dbContext.SaveChangesAsync();

                        _logger.LogInformation(
                            "PlaceOrderSaga: order created. OrderId={OrderId}", saga.CorrelationId);
                    }
                    catch (Exception ex)
                    {
                        saga.FailureReason = ex.Message;
                        _logger.LogError(ex,
                            "PlaceOrderSaga: failed to create order. OrderId={OrderId}", saga.CorrelationId);
                        throw; // MassTransit повторит попытку согласно retry policy
                    }*/