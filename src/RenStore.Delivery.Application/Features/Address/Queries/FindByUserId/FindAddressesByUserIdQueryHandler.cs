using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Address.Queries.FindByUserId;

internal sealed class FindAddressesByUserIdQueryHandler(
    IAddressQuery addressQuery,
    ILogger<FindAddressesByUserIdQueryHandler> logger)
    : IRequestHandler<FindAddressesByUserIdQuery, IReadOnlyList<AddressReadModel>>
{
    public async Task<IReadOnlyList<AddressReadModel>> Handle(
        FindAddressesByUserIdQuery request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. UserId={UserId}",
            nameof(FindAddressesByUserIdQuery), request.UserId);

        var result = await addressQuery.FindByUserIdAsync(
            request.UserId, request.IsDeleted, cancellationToken);

        logger.LogInformation(
            "Fetched {Count} addresses. UserId={UserId}",
            result.Count, request.UserId);

        return result;
    }
}