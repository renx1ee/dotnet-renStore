using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.Address.Commands.Edit;

internal sealed class EditAddressCommandHandler(
    IAddressRepository addressRepository,
    ILogger<EditAddressCommandHandler> logger)
    : IRequestHandler<EditAddressCommand>
{
    public async Task Handle(
        EditAddressCommand request,
        CancellationToken  cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. AddressId={AddressId}",
            nameof(EditAddressCommand), request.AddressId);

        var existingAddress = await addressRepository.GetAsyncById(request.AddressId, cancellationToken);
            
        if(existingAddress is null)
        {
            throw new NotFoundException(typeof(Domain.Aggregates.Address.Address), request.AddressId);
        }

        var now = DateTimeOffset.UtcNow;

        existingAddress.Edit(
            street:          request.Street,
            houseCode:       request.HouseCode,
            buildingNumber:  request.BuildingNumber,
            postcode:        request.Postcode,
            now:             now,
            apartmentNumber: request.ApartmentNumber,
            entrance:        request.Entrance,
            floor:           request.Floor);

        await addressRepository.CommitAsync(cancellationToken);
        
        logger.LogInformation(
            "Address updated. AddressId={AddressId}", request.AddressId);
    }
}