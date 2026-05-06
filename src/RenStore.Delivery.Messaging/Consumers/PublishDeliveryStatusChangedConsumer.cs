/*using MassTransit;
using Microsoft.Extensions.Logging;
using RenStore.Delivery.Contracts.IntegrationEvents;
using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Messaging.Consumers;

/// <summary>
/// Публикует интеграционное событие при каждом изменении статуса доставки.
/// Подписывается на доменные события через outbox.
/// </summary>
public sealed class PublishDeliveryStatusChangedConsumer(
    ILogger<PublishDeliveryStatusChangedConsumer> logger)
    : IConsumer<DeliveryOrderStatusChangedEvent>
{
    public async Task Consume(
        ConsumeContext<DeliveryOrderStatusChangedEvent> context)
    {
        var msg = context.Message;

        logger.LogInformation(
            "Publishing delivery status changed. DeliveryOrderId={Id} Status={Status}",
            msg.DeliveryOrderId, msg.NewStatus);

        // Здесь нам нужен OrderId — он должен быть в событии
        // В реальном проекте либо добавляем OrderId в событие,
        // либо запрашиваем из read model

        await context.Publish(new DeliveryStatusChangedIntegrationEvent(
            CorrelationId:   Guid.NewGuid(),
            DeliveryOrderId: msg.DeliveryOrderId,
            OrderId:         Guid.Empty, // заполняется через read model в реальном коде
            OldStatus:       DeliveryStatus.Placed,
            NewStatus:       msg.NewStatus,
            TrackingNumber:  null,
            CurrentLocation: msg.Location,
            OccurredAt:      msg.OccurredAt));
    }
}*/