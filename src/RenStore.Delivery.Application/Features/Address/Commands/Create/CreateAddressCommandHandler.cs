using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Address.Commands.Create;

internal sealed class CreateAddressCommandHandler(
    IAddressRepository addressRepository,
    ILogger<CreateAddressCommandHandler> logger)
    : IRequestHandler<CreateAddressCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateAddressCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. UserId={UserId}",
            nameof(CreateAddressCommand), request.UserId);

        var address = Domain.Aggregates.Address.Address.Create(
            userId:          request.UserId,
            countryId:       request.CountryId,
            cityId:          request.CityId,
            street:          request.Street,
            houseCode:       request.HouseCode,
            buildingNumber:  request.BuildingNumber,
            postcode:        request.Postcode,
            now:             DateTimeOffset.UtcNow,
            apartmentNumber: request.ApartmentNumber,
            entrance:        request.Entrance,
            floor:           request.Floor);

        await addressRepository.AddAsync(address, cancellationToken);
        await addressRepository.CommitAsync(cancellationToken);

        logger.LogInformation(
            "Address created. AddressId={AddressId} UserId={UserId}",
            address.Id, address.ApplicationUserId);

        return address.Id;
    }
}