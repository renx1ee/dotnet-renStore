namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByOrderId;

internal sealed class FindReservationByOrderIdQueryValidator
    : AbstractValidator<FindReservationByOrderIdQuery>
{
    public FindReservationByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId cannot be empty.");
    }
}