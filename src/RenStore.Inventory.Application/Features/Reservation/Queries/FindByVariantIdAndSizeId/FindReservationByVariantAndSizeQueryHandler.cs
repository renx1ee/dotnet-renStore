using RenStore.Inventory.Application.Abstractions.Queries;

namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantIdAndSizeId;

internal sealed class FindReservationByVariantAndSizeQueryHandler
    : IRequestHandler<FindReservationByVariantAndSizeQuery, VariantReservationDto?>
{
    private readonly IReservationQuery _query;
    private readonly ILogger<FindReservationByVariantAndSizeQueryHandler> _logger;

    public FindReservationByVariantAndSizeQueryHandler(
        ILogger<FindReservationByVariantAndSizeQueryHandler> logger,
        IReservationQuery query)
    {
        _query  = query;
        _logger = logger;
    }

    public async Task<VariantReservationDto?> Handle(
        FindReservationByVariantAndSizeQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(FindReservationByVariantAndSizeQuery),
            request.VariantId,
            request.SizeId);
        
        var result = await _query.FindByVariantAndSizeAsync(
            variantId: request.VariantId,
            sizeId: request.SizeId,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(FindReservationByVariantAndSizeQuery),
            request.VariantId,
            request.SizeId);

        return result;
    }
}