namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindById;

internal sealed class FindReservationByIdQueryValidator
    : AbstractValidator<FindReservationByIdQuery>
{
    public FindReservationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ReservationId cannot be empty.");
    }
}