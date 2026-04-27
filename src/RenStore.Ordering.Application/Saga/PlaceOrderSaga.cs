using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Saga.Commands;
using RenStore.Order.Application.Saga.Events;
using RenStore.Ordering.Contracts.Requests;

namespace RenStore.Order.Application.Saga;

public sealed class PlaceOrderSaga : MassTransitStateMachine<PlaceOrderSagaState>
{
    private readonly ILogger<PlaceOrderSaga> _logger;

    public State WaitingForExternalData { get; private set; } = null!;
    public State CreatingOrder          { get; private set; } = null!;
    public State Completed              { get; private set; } = null!;
    public State Failed                 { get; private set; } = null!;

    public Event<InitiateOrderPlacement> OrderInitiated  { get; private set; } = null!;
    public Event<VariantPriceReceived>   PriceReceived   { get; private set; } = null!;
    public Event<VariantPriceFailed>     PriceFailed     { get; private set; } = null!;
    public Event<ShippingAddressReceived> AddressReceived { get; private set; } = null!;
    public Event<ShippingAddressFailed>  AddressFailed   { get; private set; } = null!;
    public Event<OrderCreated>           OrderCreated    { get; private set; } = null!;
    public Event<OrderCreationFailed>    OrderFailed     { get; private set; } = null!;

    public Schedule<PlaceOrderSagaState, SagaTimeout> ResponseTimeout { get; private set; } = null!;

    public PlaceOrderSaga(ILogger<PlaceOrderSaga> logger)
    {
        _logger = logger;

        InstanceState(x => x.CurrentState);
        ConfigureEvents();
        ConfigureSchedule();
        ConfigureStateMachine();
    }

    private void ConfigureEvents()
    {
        Event(() => OrderInitiated,  x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        Event(() => PriceReceived,   x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        Event(() => PriceFailed,     x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        Event(() => AddressReceived, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        Event(() => AddressFailed,   x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        Event(() => OrderCreated,    x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        Event(() => OrderFailed,     x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
    }

    private void ConfigureSchedule()
    {
        Schedule(
            () => ResponseTimeout,
            x => x.TimeoutTokenId,
            s =>
            {
                s.Delay = TimeSpan.FromSeconds(30);
                s.Received = r => r.CorrelateById(ctx => ctx.Message.CorrelationId);
            });
    }

    private void ConfigureStateMachine()
    {
        // ─── Initial → WaitingForExternalData ────────────────────────────────────
        Initially(
            When(OrderInitiated)
                .Then(ctx =>
                {
                    ctx.Saga.CustomerId = ctx.Message.CustomerId;
                    ctx.Saga.VariantId  = ctx.Message.VariantId;
                    ctx.Saga.SizeId     = ctx.Message.SizeId;
                    ctx.Saga.Quantity   = ctx.Message.Quantity;

                    _logger.LogInformation(
                        "PlaceOrderSaga started. CorrelationId={CorrelationId} CustomerId={CustomerId}",
                        ctx.Saga.CorrelationId, ctx.Saga.CustomerId);
                })
                .PublishAsync(ctx => ctx.Init<GetVariantSizePriceRequest>(new GetVariantSizePriceRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId)))
                .PublishAsync(ctx => ctx.Init<GetShippingAddressRequest>(new GetShippingAddressRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    CustomerId:    ctx.Saga.CustomerId)))
                .Schedule(ResponseTimeout,
                    ctx => ctx.Init<SagaTimeout>(new SagaTimeout(ctx.Saga.CorrelationId)))
                .TransitionTo(WaitingForExternalData));
        
        During(WaitingForExternalData,

            When(PriceReceived)
                .Then(ctx =>
                {
                    ctx.Saga.PriceAmount         = ctx.Message.PriceAmount;
                    ctx.Saga.Currency            = ctx.Message.Currency;
                    ctx.Saga.ProductNameSnapshot = ctx.Message.ProductNameSnapshot;
                    ctx.Saga.PriceReceived       = true;

                    _logger.LogDebug(
                        "Price received. CorrelationId={CorrelationId} Price={Price}",
                        ctx.Saga.CorrelationId, ctx.Message.PriceAmount);
                })
                .IfElse(
                    ctx => ctx.Saga.AddressReceived,
                    ready   => ready.Unschedule(ResponseTimeout).TransitionTo(CreatingOrder),
                    waiting => waiting.TransitionTo(WaitingForExternalData)),

            When(AddressReceived)
                .Then(ctx =>
                {
                    ctx.Saga.ShippingAddress = ctx.Message.ShippingAddress;
                    ctx.Saga.AddressReceived = true;

                    _logger.LogDebug(
                        "Address received. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                .IfElse(
                    ctx => ctx.Saga.PriceReceived,
                    ready   => ready.Unschedule(ResponseTimeout).TransitionTo(CreatingOrder),
                    waiting => waiting.TransitionTo(WaitingForExternalData)),

            When(PriceFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Catalog service failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "Price fetch failed. CorrelationId={CorrelationId} Reason={Reason}",
                        ctx.Saga.CorrelationId, ctx.Message.Reason);
                })
                .Unschedule(ResponseTimeout)
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx
                    .Init<OrderPlacementFailed>(new OrderPlacementFailed(
                        CorrelationId: ctx.Saga.CorrelationId,
                        Reason:        ctx.Saga.FailureReason!)))
                .Finalize(),

            When(AddressFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Delivery service failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "Address fetch failed. CorrelationId={CorrelationId} Reason={Reason}",
                        ctx.Saga.CorrelationId, ctx.Message.Reason);
                })
                .Unschedule(ResponseTimeout)
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize(),

            When(ResponseTimeout.Received)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason =
                        $"Timeout. PriceReceived={ctx.Saga.PriceReceived}, " +
                        $"AddressReceived={ctx.Saga.AddressReceived}";
                    _logger.LogError(
                        "Saga timeout. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize());
        
        // Входим — публикуем команду консьюмеру, который сделает всю работу с БД
        WhenEnter(CreatingOrder, binder => binder
            .Then(ctx => _logger.LogInformation(
                "Both responses received, sending CreateOrderCommand. CorrelationId={CorrelationId}",
                ctx.Saga.CorrelationId))
            .PublishAsync(ctx => ctx.Init<CreateOrderCommand>(new CreateOrderCommand(
                CorrelationId:       ctx.Saga.CorrelationId,
                CustomerId:          ctx.Saga.CustomerId,
                VariantId:           ctx.Saga.VariantId,
                SizeId:              ctx.Saga.SizeId,
                Quantity:            ctx.Saga.Quantity,
                PriceAmount:         ctx.Saga.PriceAmount!.Value,
                Currency:            ctx.Saga.Currency!,
                ProductNameSnapshot: ctx.Saga.ProductNameSnapshot!,
                ShippingAddress:     ctx.Saga.ShippingAddress!))));

        During(CreatingOrder,

            When(OrderCreated)
                .Then(ctx =>
                {
                    ctx.Saga.OrderId = ctx.Message.OrderId;
                    _logger.LogInformation(
                        "Order created successfully. CorrelationId={CorrelationId} OrderId={OrderId}",
                        ctx.Saga.CorrelationId, ctx.Message.OrderId);
                })
                .TransitionTo(Completed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementCompleted>(new OrderPlacementCompleted(
                    CorrelationId: ctx.Saga.CorrelationId,
                    OrderId:       ctx.Saga.OrderId!.Value)))
                .Finalize(),

            When(OrderFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = ctx.Message.Reason;
                    _logger.LogError(
                        "Order creation failed. CorrelationId={CorrelationId} Reason={Reason}",
                        ctx.Saga.CorrelationId, ctx.Message.Reason);
                })
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize());

        SetCompletedWhenFinalized();
    }
}