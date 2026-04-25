using RenStore.Inventory.Application.Abstractions.Queries;

namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindById;

internal sealed class FindReservationByIdQueryHandler
    : IRequestHandler<FindReservationByIdQuery, VariantReservationDto?>
{
    private readonly IReservationQuery _query;
    private readonly ILogger<FindReservationByIdQueryHandler> _logger;

    public FindReservationByIdQueryHandler(
        ILogger<FindReservationByIdQueryHandler> logger,
        IReservationQuery query)
    {
        _query  = query;
        _logger = logger;
    }

    public async Task<VariantReservationDto?> Handle(
        FindReservationByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} ReservationId: {ReservationId}",
            nameof(FindReservationByIdQuery),
            request.Id);
        
        var result = await _query.FindByIdAsync(request.Id, cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. ReservationId: {ReservationId}",
            nameof(FindReservationByIdQuery),
            request.Id);

        return result;
    }
}