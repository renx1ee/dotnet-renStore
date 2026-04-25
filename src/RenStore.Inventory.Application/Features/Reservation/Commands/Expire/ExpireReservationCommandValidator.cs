namespace RenStore.Inventory.Application.Features.Reservation.Commands.Expire;

internal sealed class ExpireReservationCommandValidator
    : AbstractValidator<ExpireReservationCommand>
{
    public ExpireReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty()
            .WithMessage("ReservationId cannot be empty.");
    }
}