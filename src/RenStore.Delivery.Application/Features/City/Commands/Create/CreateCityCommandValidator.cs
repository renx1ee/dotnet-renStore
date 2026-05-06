namespace RenStore.Delivery.Application.Features.City.Commands.Create;

internal sealed class CreateCityCommandValidator
    : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NameRu).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CountryId).GreaterThan(0);
    }
}