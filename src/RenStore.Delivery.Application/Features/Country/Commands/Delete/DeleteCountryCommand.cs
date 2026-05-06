namespace RenStore.Delivery.Application.Features.Country.Commands.Delete;

public sealed record DeleteCountryCommand(int CountryId) : IRequest;