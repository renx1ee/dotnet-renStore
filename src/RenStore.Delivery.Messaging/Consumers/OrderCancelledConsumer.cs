using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.DeleteDeliveryOrder;
using RenStore.Delivery.Contracts.IntegrationEvents;
using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Messaging.Consumers;

/// <summary>
/// Слушает отмену заказа.
/// Если доставка ещё не передана Почте — удаляем.
/// Если уже в пути — логируем, ручная обработка.
/// </summary>
public sealed class OrderCancelledConsumer(
    IMediator           mediator,
    IDeliveryOrderQuery deliveryOrderQuery,
    ILogger<OrderCancelledConsumer> logger)
    : IConsumer<OrderCancelledIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<OrderCancelledIntegrationEvent> context)
    {
        var msg = context.Message;

        logger.LogInformation(
            "Received {Event}. OrderId={OrderId} Reason={Reason}",
            nameof(OrderCancelledIntegrationEvent),
            msg.OrderId,
            msg.Reason);

        var delivery = await deliveryOrderQuery.FindByOrderIdAsync(
            msg.OrderId,
            context.CancellationToken);

        if (delivery is null)
        {
            logger.LogWarning(
                "Delivery not found for cancelled order. OrderId={OrderId}",
                msg.OrderId);
            return;
        }

        if (delivery.Status == DeliveryStatus.Placed)
        {
            // Ещё не зарегистрировано в Почте — просто удаляем
            await mediator.Send(
                new DeleteDeliveryOrderCommand(delivery.Id),
                context.CancellationToken);

            logger.LogInformation(
                "Delivery deleted for cancelled order. DeliveryOrderId={Id} OrderId={OrderId}",
                delivery.Id, msg.OrderId);
        }
        else
        {
            // Уже в пути — требуется ручная обработка возврата
            logger.LogWarning(
                "Cannot auto-cancel delivery — already in transit. " +
                "DeliveryOrderId={Id} OrderId={OrderId} Status={Status}. " +
                "Manual return process required.",
                delivery.Id, msg.OrderId, delivery.Status);

            // Публикуем событие для нотификации команды
            await context.Publish(new DeliveryReturnRequiredIntegrationEvent(
                CorrelationId:   msg.CorrelationId,
                DeliveryOrderId: delivery.Id,
                OrderId:         msg.OrderId,
                TrackingNumber:  delivery.TrackingNumber,
                CurrentStatus:   delivery.Status.ToString()));
        }
    }
}