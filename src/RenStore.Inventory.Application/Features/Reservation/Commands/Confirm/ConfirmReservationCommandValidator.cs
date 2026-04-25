namespace RenStore.Inventory.Application.Features.Reservation.Commands.Confirm;

internal sealed class ConfirmReservationCommandValidator
    : AbstractValidator<ConfirmReservationCommand>
{
    public ConfirmReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty()
            .WithMessage("Reservation ID cannot be empty guid.");
    }
}