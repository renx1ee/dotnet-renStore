namespace RenStore.Delivery.Application.Features.Country.Commands.Create;

public sealed class CreateCountryCommandValidator
    : AbstractValidator<CreateCountryCommand>
{
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NameRu).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().Length(2)
            .Matches(@"^[A-Z]{2}$").WithMessage("Code must be 2 uppercase letters (ISO 3166-1).");
        RuleFor(x => x.PhoneCode).NotEmpty().MaximumLength(4)
            .Matches(@"^\+?\d+$").WithMessage("PhoneCode must be digits with optional +.");
    }
}