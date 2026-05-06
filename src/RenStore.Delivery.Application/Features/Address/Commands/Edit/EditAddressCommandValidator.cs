/*namespace RenStore.Delivery.Application.Features.Address.Commands.Edit;

internal sealed class EditAddressCommandValidator
    : AbstractValidator<EditAddressCommand>
{
    public EditAddressCommandValidator()
    {
        RuleFor(x => x.AddressId).NotEmpty();
        RuleFor(x => x.Street).NotEmpty().MaximumLength(256);
        RuleFor(x => x.HouseCode).NotEmpty().MaximumLength(32);
        RuleFor(x => x.BuildingNumber).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Postcode).NotEmpty().Length(6)
            .Matches(@"^\d{6}$").WithMessage("Postcode must be 6 digits.");
        RuleFor(x => x.Floor).InclusiveBetween(1, 200).When(x => x.Floor != null);
    }
}*/