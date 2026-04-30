using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Features.Order.Commands.Create;
using RenStore.Order.Application.Saga.Contracts.Events;

namespace RenStore.Ordering.Messaging.Consumers;

internal sealed class CreateOrderConsumer 
    (IMediator mediator,
    ILogger<CreateOrderConsumer> logger)
    : IConsumer<CreateOrderCommand>
{
    public async Task Consume(ConsumeContext<CreateOrderCommand> context)
    {
        var msg = context.Message;

        try
        {
            var orderId = await mediator.Send(
                new RenStore.Order.Application.Features.Order.Commands.Create.CreateOrderCommand(
                    CorrelationId:       msg.CorrelationId,
                    CustomerId:          msg.CustomerId,
                    VariantId:           msg.VariantId,
                    SizeId:              msg.SizeId,
                    Quantity:            msg.Quantity,
                    PriceAmount:         msg.PriceAmount,
                    Currency:            msg.Currency,
                    ProductNameSnapshot: msg.ProductNameSnapshot,
                    ShippingAddress:     msg.ShippingAddress),
                context.CancellationToken);

            await context.Publish(new OrderCreated(
                CorrelationId: msg.CorrelationId,
                OrderId:       orderId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                message: "Failed to create order. CorrelationId={CorrelationId}",
                msg.CorrelationId);

            await context.Publish(new OrderCreationFailed(
                CorrelationId: msg.CorrelationId,
                Reason:        ex.Message));
        }
    }
}