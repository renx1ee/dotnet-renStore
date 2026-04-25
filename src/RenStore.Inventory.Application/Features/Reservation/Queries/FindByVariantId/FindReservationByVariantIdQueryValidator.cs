namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantId;

internal sealed class FindReservationByVariantIdQueryValidator
    : AbstractValidator<FindReservationByVariantIdQuery>
{
    public FindReservationByVariantIdQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("VariantId cannot be empty.");

        RuleFor(x => x.Page)
            .GreaterThan(0u)
            .WithMessage("Page must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1u, 100u)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}