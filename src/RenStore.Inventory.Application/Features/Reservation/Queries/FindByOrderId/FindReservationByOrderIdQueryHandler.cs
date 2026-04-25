using RenStore.Inventory.Application.Abstractions.Queries;

namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByOrderId;

internal sealed class FindReservationByOrderIdQueryHandler
    : IRequestHandler<FindReservationByOrderIdQuery, IReadOnlyList<VariantReservationDto>>
{
    private readonly IReservationQuery _query;
    private readonly ILogger<FindReservationByOrderIdQueryHandler> _logger;

    public FindReservationByOrderIdQueryHandler(
        ILogger<FindReservationByOrderIdQueryHandler> logger,
        IReservationQuery query)
    {
        _query  = query;
        _logger = logger;
    }

    public async Task<IReadOnlyList<VariantReservationDto>> Handle(
        FindReservationByOrderIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} OrderId: {OrderId}",
            nameof(FindReservationByOrderIdQuery),
            request.OrderId);

        var result = await _query.FindByOrderIdAsync(
            orderId: request.OrderId,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. OrderId: {OrderId}",
            nameof(FindReservationByOrderIdQuery),
            request.OrderId);

        return result;
    }
}