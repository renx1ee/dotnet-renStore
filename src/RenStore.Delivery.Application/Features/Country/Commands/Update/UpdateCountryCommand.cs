namespace RenStore.Delivery.Application.Features.Country.Commands.Update;

public sealed record UpdateCountryCommand(
    int    CountryId,
    string Name,
    string NameRu,
    string Code,
    string PhoneCode) : IRequest;