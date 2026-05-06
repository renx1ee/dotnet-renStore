using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.RegisterWithRussianPost;
using RenStore.Delivery.Application.Features.DeliveryOrder.Create;
using RenStore.Delivery.Contracts.IntegrationEvents;

namespace RenStore.Delivery.Messaging.Consumers;

/// <summary>
/// Слушает событие завершения оформления заказа.
/// Автоматически создаёт DeliveryOrder и регистрирует его в Почте России.
/// </summary>
public sealed class OrderPlacementCompletedConsumer(
    IMediator mediator,
    ILogger<OrderPlacementCompletedConsumer> logger)
    : IConsumer<OrderPlacementCompletedIntegrationEvent>
{
    // Тариф по умолчанию — в реальном проекте выбирается динамически
    // на основе веса, суммы заказа и типа тарифа
    private const int DefaultTariffId = 1;

    public async Task Consume(
        ConsumeContext<OrderPlacementCompletedIntegrationEvent> context)
    {
        var msg = context.Message;

        logger.LogInformation(
            "Received {Event}. OrderId={OrderId} CustomerId={CustomerId}",
            nameof(OrderPlacementCompletedIntegrationEvent),
            msg.OrderId,
            msg.CustomerId);

        try
        {
            // 1. Создаём DeliveryOrder
            var deliveryOrderId = await mediator.Send(
                new CreateDeliveryOrderCommand(
                    OrderId:          msg.OrderId,
                    DeliveryTariffId: DefaultTariffId),
                context.CancellationToken);

            // 2. Регистрируем в Почте России — получаем трек-номер
            var trackingNumber = await mediator.Send(
                new RegisterWithRussianPostCommand(
                    DeliveryOrderId: deliveryOrderId,
                    RecipientName:   msg.RecipientName,
                    RecipientPhone:  msg.RecipientPhone,
                    AddressTo:       msg.ShippingAddress,
                    PostcodeFrom:    "101000", // индекс нашего склада — из конфига
                    PostcodeTo:      msg.ShippingPostcode,
                    WeightGrams:     msg.TotalWeightGrams,
                    ValueRub:        msg.TotalValueRub),
                context.CancellationToken);

            // 3. Публикуем событие — доставка создана
            await context.Publish(new DeliveryOrderCreatedIntegrationEvent(
                CorrelationId:   msg.CorrelationId,
                DeliveryOrderId: deliveryOrderId,
                OrderId:         msg.OrderId,
                TariffId:        DefaultTariffId));

            // 4. Публикуем событие — трек-номер получен
            await context.Publish(new DeliveryOrderRegisteredIntegrationEvent(
                CorrelationId:   msg.CorrelationId,
                DeliveryOrderId: deliveryOrderId,
                OrderId:         msg.OrderId,
                TrackingNumber:  trackingNumber));
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create delivery for OrderId={OrderId}",
                msg.OrderId);
            throw; // MassTransit повторит попытку согласно retry policy
        }
    }
}