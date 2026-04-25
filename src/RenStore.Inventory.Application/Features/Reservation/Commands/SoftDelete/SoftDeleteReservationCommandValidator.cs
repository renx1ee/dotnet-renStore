namespace RenStore.Inventory.Application.Features.Reservation.Commands.SoftDelete;

internal sealed class SoftDeleteReservationCommandValidator
    : AbstractValidator<SoftDeleteReservationCommand>
{
    public SoftDeleteReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty()
            .WithMessage("ReservationId cannot be empty.");
    }
}