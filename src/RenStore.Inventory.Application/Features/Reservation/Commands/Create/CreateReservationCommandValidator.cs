namespace RenStore.Inventory.Application.Features.Reservation.Commands.Create;

internal sealed class CreateReservationCommandValidator
    : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("VariantId cannot be empty.");

        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("SizeId cannot be empty.");

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId cannot be empty.");
    }
}