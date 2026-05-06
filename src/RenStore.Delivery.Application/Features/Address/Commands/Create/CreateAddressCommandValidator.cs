namespace RenStore.Delivery.Application.Features.Address.Commands.Create;

internal sealed class CreateAddressCommandValidator
    : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CountryId).GreaterThan(0);
        RuleFor(x => x.CityId).GreaterThan(0);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(256);
        RuleFor(x => x.HouseCode).NotEmpty().MaximumLength(32);
        RuleFor(x => x.BuildingNumber).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Postcode).NotEmpty().Length(6)
            .Matches(@"^\d{6}$").WithMessage("Postcode must be 6 digits.");
        RuleFor(x => x.ApartmentNumber).MaximumLength(32).When(x => x.ApartmentNumber != null);
        RuleFor(x => x.Entrance).MaximumLength(32).When(x => x.Entrance != null);
        RuleFor(x => x.Floor).InclusiveBetween(1, 200).When(x => x.Floor != null);
    }
}