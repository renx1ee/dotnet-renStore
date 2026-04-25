using MassTransit;
using MassTransit.Mediator;
using RenStore.Inventory.Application.Features.Reservation.Commands.Confirm;
using RenStore.Inventory.Contracts.Events;

namespace RenStore.Inventory.Messaging.Consumers;

internal sealed class ConfirmReservationConsumer(IMediator mediator)
    : IConsumer<ConfirmReservationIntegrationEvent>
{
    private readonly IMediator _mediator = mediator;
    
    public async Task Consume(ConsumeContext<ConfirmReservationIntegrationEvent> context)
    {
        await _mediator.Send(
            new ConfirmReservationCommand(
                ReservationId: context.Message.ReservationId),
            context.CancellationToken);
    }
}