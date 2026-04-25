namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantIdAndSizeId;

internal sealed class FindReservationByVariantAndSizeQueryValidator
    : AbstractValidator<FindReservationByVariantAndSizeQuery>
{
    public FindReservationByVariantAndSizeQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("VariantId cannot be empty.");

        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("SizeId cannot be empty.");
    }
}