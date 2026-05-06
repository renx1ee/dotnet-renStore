namespace RenStore.Delivery.Application.Features.City.Commands.Update;

public sealed class UpdateCityCommandValidator 
    : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.CityId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NameRu).NotEmpty().MaximumLength(100);
    }
}