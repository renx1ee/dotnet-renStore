using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Saga;
using RenStore.Order.Application.Saga.Contracts.Commands;
using RenStore.Order.Application.Saga.Contracts.Events;
using GetShippingAddressRequest = RenStore.Ordering.Contracts.Requests.GetShippingAddressRequest;

public sealed class PlaceOrderSaga : MassTransitStateMachine<PlaceOrderSagaState>
{
    private readonly ILogger<PlaceOrderSaga> _logger;

    // States
    public State ReservingStock       { get; private set; } = null!;
    public State WaitingForData       { get; private set; } = null!;
    public State CreatingOrder        { get; private set; } = null!;
    public State WaitingForPayment    { get; private set; } = null!;
    public State Completed            { get; private set; } = null!;
    public State Failed               { get; private set; } = null!;

    // Events
    public Event<InitiateOrderPlacement>  OrderInitiated   { get; private set; } = null!;
    public Event<StockReserved>           StockReserved    { get; private set; } = null!;
    public Event<StockReservationFailed>  StockFailed      { get; private set; } = null!;
    public Event<VariantSnapshotReceived> SnapshotReceived { get; private set; } = null!;
    public Event<VariantSnapshotFailed>   SnapshotFailed   { get; private set; } = null!;
    public Event<ShippingAddressReceived> AddressReceived  { get; private set; } = null!;
    public Event<ShippingAddressFailed>   AddressFailed    { get; private set; } = null!;
    public Event<OrderCreated>            OrderCreated     { get; private set; } = null!;
    public Event<OrderCreationFailed>     OrderFailed      { get; private set; } = null!;
    public Event<PaymentCompleted>        PaymentCompleted { get; private set; } = null!;
    public Event<PaymentFailed>           PaymentFailed    { get; private set; } = null!;

    public Schedule<PlaceOrderSagaState, SagaTimeout> ExternalDataTimeout { get; private set; } = null!;
    public Schedule<PlaceOrderSagaState, SagaTimeout> PaymentTimeout      { get; private set; } = null!;

    public PlaceOrderSaga(ILogger<PlaceOrderSaga> logger)
    {
        _logger = logger;

        InstanceState(x => x.CurrentState);
        ConfigureEvents();
        ConfigureSchedules();
        ConfigureStateMachine();
    }

    private void ConfigureEvents()
    {
        Event(() => OrderInitiated,   x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => StockReserved,    x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => StockFailed,      x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => SnapshotReceived, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => SnapshotFailed,   x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => AddressReceived,  x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => AddressFailed,    x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => OrderCreated,     x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => OrderFailed,      x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => PaymentFailed,    x => x.CorrelateById(m => m.Message.CorrelationId));
    }

    private void ConfigureSchedules()
    {
        // Таймаут ожидания снепшота и адреса — 30 секунд
        Schedule(() => ExternalDataTimeout,
            x => x.ExternalDataTimeoutId,
            s =>
            {
                s.Delay = TimeSpan.FromSeconds(30);
                s.Received = r => r.CorrelateById(m => m.Message.CorrelationId);
            });

        // Таймаут ожидания оплаты — 15 минут
        Schedule(() => PaymentTimeout,
            x => x.PaymentTimeoutId,
            s =>
            {
                s.Delay = TimeSpan.FromMinutes(15);
                s.Received = r => r.CorrelateById(m => m.Message.CorrelationId);
            });
    }

    private void ConfigureStateMachine()
    {
        // ── Initial → ReservingStock ─────────────────────────────────────────────
        Initially(
            When(OrderInitiated)
                .Then(ctx =>
                {
                    ctx.Saga.CustomerId = ctx.Message.CustomerId;
                    ctx.Saga.VariantId  = ctx.Message.VariantId;
                    ctx.Saga.SizeId     = ctx.Message.SizeId;
                    ctx.Saga.Quantity   = ctx.Message.Quantity;

                    _logger.LogInformation(
                        "PlaceOrderSaga started. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                .PublishAsync(ctx => ctx.Init<ReserveStockRequest>(new ReserveStockRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId,
                    Quantity:      ctx.Saga.Quantity)))
                .TransitionTo(ReservingStock));

        // ── ReservingStock ───────────────────────────────────────────────────────
        During(ReservingStock,

            When(StockReserved)
                .Then(ctx => _logger.LogInformation(
                    "Stock reserved. CorrelationId={CorrelationId}",
                    ctx.Saga.CorrelationId))
                // Параллельно запрашиваем снепшот и адрес
                .PublishAsync(ctx => ctx.Init<GetVariantSnapshotRequest>(new GetVariantSnapshotRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId)))
                .PublishAsync(ctx => ctx.Init<GetShippingAddressRequest>(new GetShippingAddressRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    CustomerId:    ctx.Saga.CustomerId)))
                .Schedule(ExternalDataTimeout,
                    ctx => ctx.Init<SagaTimeout>(new SagaTimeout(ctx.Saga.CorrelationId)))
                .TransitionTo(WaitingForData),

            When(StockFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Stock reservation failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "Stock reservation failed. CorrelationId={CorrelationId} Reason={Reason}",
                        ctx.Saga.CorrelationId, ctx.Message.Reason);
                })
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize());

        // ── WaitingForData ───────────────────────────────────────────────────────
        During(WaitingForData,

            When(SnapshotReceived)
                .Then(ctx =>
                {
                    ctx.Saga.PriceAmount         = ctx.Message.PriceAmount;
                    ctx.Saga.Currency            = ctx.Message.Currency;
                    ctx.Saga.ProductNameSnapshot = ctx.Message.ProductNameSnapshot;
                    ctx.Saga.SnapshotReceived    = true;

                    _logger.LogDebug(
                        "Snapshot received. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                .IfElse(
                    ctx => ctx.Saga.AddressReceived,
                    ready   => ready.Unschedule(ExternalDataTimeout).TransitionTo(CreatingOrder),
                    waiting => waiting.TransitionTo(WaitingForData)),

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
                    ctx => ctx.Saga.SnapshotReceived,
                    ready   => ready.Unschedule(ExternalDataTimeout).TransitionTo(CreatingOrder),
                    waiting => waiting.TransitionTo(WaitingForData)),

            When(SnapshotFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Catalog service failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "Snapshot failed. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                .Unschedule(ExternalDataTimeout)
                // Компенсация — снимаем резерв
                .PublishAsync(ctx => ctx.Init<ReleaseStockRequest>(new ReleaseStockRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId,
                    Quantity:      ctx.Saga.Quantity)))
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize(),

            When(AddressFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Delivery service failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "Address failed. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                .Unschedule(ExternalDataTimeout)
                // Компенсация — снимаем резерв
                .PublishAsync(ctx => ctx.Init<ReleaseStockRequest>(new ReleaseStockRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId,
                    Quantity:      ctx.Saga.Quantity)))
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize(),

            When(ExternalDataTimeout.Received)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason =
                        $"Timeout. SnapshotReceived={ctx.Saga.SnapshotReceived}, " +
                        $"AddressReceived={ctx.Saga.AddressReceived}";
                    _logger.LogError(
                        "External data timeout. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                // Компенсация — снимаем резерв
                .PublishAsync(ctx => ctx.Init<ReleaseStockRequest>(new ReleaseStockRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId,
                    Quantity:      ctx.Saga.Quantity)))
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize());

        // ── CreatingOrder ────────────────────────────────────────────────────────
        WhenEnter(CreatingOrder, binder => binder
            .Then(ctx => _logger.LogInformation(
                "Creating order. CorrelationId={CorrelationId}",
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
                        "Order created, waiting for payment. CorrelationId={CorrelationId} OrderId={OrderId}",
                        ctx.Saga.CorrelationId, ctx.Message.OrderId);
                })
                // Запрашиваем оплату
                .PublishAsync(ctx => ctx.Init<ProcessPaymentRequest>(new ProcessPaymentRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    OrderId:       ctx.Saga.OrderId!.Value,
                    CustomerId:    ctx.Saga.CustomerId,
                    Amount:        ctx.Saga.PriceAmount!.Value,
                    Currency:      ctx.Saga.Currency!)))
                .Schedule(PaymentTimeout,
                    ctx => ctx.Init<SagaTimeout>(new SagaTimeout(ctx.Saga.CorrelationId)))
                .TransitionTo(WaitingForPayment),

            When(OrderFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = ctx.Message.Reason;
                    _logger.LogError(
                        "Order creation failed. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                // Компенсация — снимаем резерв
                .PublishAsync(ctx => ctx.Init<ReleaseStockRequest>(new ReleaseStockRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId,
                    Quantity:      ctx.Saga.Quantity)))
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize());

        // ── WaitingForPayment ────────────────────────────────────────────────────
        During(WaitingForPayment,

            When(PaymentCompleted)
                .Then(ctx => _logger.LogInformation(
                    "Payment completed. CorrelationId={CorrelationId} OrderId={OrderId}",
                    ctx.Saga.CorrelationId, ctx.Saga.OrderId))
                .Unschedule(PaymentTimeout)
                .TransitionTo(Completed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementCompleted>(new OrderPlacementCompleted(
                    CorrelationId: ctx.Saga.CorrelationId,
                    OrderId:       ctx.Saga.OrderId!.Value)))
                .Finalize(),

            When(PaymentFailed)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = $"Payment failed: {ctx.Message.Reason}";
                    _logger.LogWarning(
                        "Payment failed. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                .Unschedule(PaymentTimeout)
                // Компенсация — снимаем резерв и отменяем заказ
                .PublishAsync(ctx => ctx.Init<ReleaseStockRequest>(new ReleaseStockRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId,
                    Quantity:      ctx.Saga.Quantity)))
                .PublishAsync(ctx => ctx.Init<CancelOrderCommand>(new CancelOrderCommand(
                    CorrelationId: ctx.Saga.CorrelationId,
                    OrderId:       ctx.Saga.OrderId!.Value,
                    Reason:        ctx.Saga.FailureReason!)))
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize(),

            When(PaymentTimeout.Received)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = "Payment timeout.";
                    _logger.LogWarning(
                        "Payment timeout. CorrelationId={CorrelationId}",
                        ctx.Saga.CorrelationId);
                })
                // Компенсация — снимаем резерв и отменяем заказ
                .PublishAsync(ctx => ctx.Init<ReleaseStockRequest>(new ReleaseStockRequest(
                    CorrelationId: ctx.Saga.CorrelationId,
                    VariantId:     ctx.Saga.VariantId,
                    SizeId:        ctx.Saga.SizeId,
                    Quantity:      ctx.Saga.Quantity)))
                .PublishAsync(ctx => ctx.Init<CancelOrderCommand>(new CancelOrderCommand(
                    CorrelationId: ctx.Saga.CorrelationId,
                    OrderId:       ctx.Saga.OrderId!.Value,
                    Reason:        "Payment timeout")))
                .TransitionTo(Failed)
                .PublishAsync(ctx => ctx.Init<OrderPlacementFailed>(new OrderPlacementFailed(
                    CorrelationId: ctx.Saga.CorrelationId,
                    Reason:        ctx.Saga.FailureReason!)))
                .Finalize());

        SetCompletedWhenFinalized();
    }
}