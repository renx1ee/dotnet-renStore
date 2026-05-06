namespace RenStore.Delivery.Application.Features.Country.Commands.Create;

public sealed record CreateCountryCommand(
    string Name,
    string NameRu,
    string Code,
    string PhoneCode) 
    : IRequest<int>;