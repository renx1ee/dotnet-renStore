namespace RenStore.Delivery.Application.Features.Address.Commands.Delete;

public sealed record DeleteAddressCommand(Guid AddressId) : IRequest;