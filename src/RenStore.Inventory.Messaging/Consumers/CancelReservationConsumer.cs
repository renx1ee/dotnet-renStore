using MassTransit;
using MassTransit.Mediator;
using RenStore.Inventory.Application.Features.Reservation.Commands.Cancel;
using RenStore.Inventory.Contracts.Events;

namespace RenStore.Inventory.Messaging.Consumers;

internal sealed class CancelReservationConsumer(IMediator mediator)
    : IConsumer<CancelReservationIntegrationEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<CancelReservationIntegrationEvent> context)
    {
        await _mediator.Send(
            new CancelReservationCommand(
                ReservationId: context.Message.ReservationId,
                Reason: context.Message.Reason),
            context.CancellationToken);
    }
}