namespace RenStore.Inventory.Application.Features.Reservation.Commands.Cancel;

internal sealed class CancelReservationCommandValidator
    : AbstractValidator<CancelReservationCommand>
{
    public CancelReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty()
            .WithMessage("ReservationId cannot be empty.");

        RuleFor(x => x.Reason)
            .IsInEnum()
            .WithMessage("Invalid cancel reason.");
    }
}