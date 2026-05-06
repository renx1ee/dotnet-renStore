using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Address.Queries.FindById;

internal sealed class FindAddressByIdQueryHandler(
    IAddressQuery addressQuery,
    ILogger<FindAddressByIdQueryHandler> logger)
    : IRequestHandler<FindAddressByIdQuery, AddressReadModel?>
{
    public async Task<AddressReadModel?> Handle(
        FindAddressByIdQuery request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. AddressId={AddressId}",
            nameof(FindAddressByIdQuery), request.AddressId);

        var result = await addressQuery.FindByIdAsync(request.AddressId, cancellationToken);

        if (result is null)
            logger.LogWarning("Address not found. AddressId={AddressId}", request.AddressId);

        return result;
    }
}