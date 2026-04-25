using MassTransit;
using MediatR;
using RenStore.Inventory.Application.Features.Reservation.Commands.Create;
using RenStore.Inventory.Contracts.Events;
using RenStore.Inventory.Contracts.Renponses;

namespace RenStore.Inventory.Messaging.Consumers;

internal sealed class CreateReservationRequestConsumer(IMediator mediator)
    : IConsumer<CreateReservationIntegrationEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<CreateReservationIntegrationEvent> context)
    {
        var message = context.Message;

        var reservationId = await _mediator.Send(
            new CreateReservationCommand(
                VariantId: message.VariantId,
                SizeId:    message.SizeId,
                OrderId:   message.OrderId,
                Quantity:  message.Quantity),
            context.CancellationToken);

        await context.RespondAsync(new ReservationCreatedResponse(
            ReservationId: reservationId));
    }
}