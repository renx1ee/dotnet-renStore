using RenStore.Inventory.Application.Abstractions.Queries;

namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindExpired;

internal sealed class FindExpiredReservationsQueryHandler
    : IRequestHandler<FindExpiredReservationsQuery, IReadOnlyList<VariantReservationDto>>
{
    private readonly IReservationQuery _query;
    private readonly ILogger<FindExpiredReservationsQueryHandler> _logger;

    public FindExpiredReservationsQueryHandler(
        ILogger<FindExpiredReservationsQueryHandler> logger,
        IReservationQuery query)
    {
        _query  = query;
        _logger = logger;
    }

    public async Task<IReadOnlyList<VariantReservationDto>> Handle(
        FindExpiredReservationsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query}", 
            nameof(FindExpiredReservationsQuery));
        
        var result = await _query.FindExpiredReservationsAsync(cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. ", 
            nameof(FindExpiredReservationsQuery));

        return result;
    }
}