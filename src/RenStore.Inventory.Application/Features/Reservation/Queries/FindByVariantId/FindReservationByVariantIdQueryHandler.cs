using RenStore.Inventory.Application.Abstractions.Queries;

namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantId;

internal sealed class FindReservationByVariantIdQueryHandler
    : IRequestHandler<FindReservationByVariantIdQuery, IReadOnlyList<VariantReservationDto>>
{
    private readonly IReservationQuery _query;
    private readonly ILogger<FindReservationByVariantIdQueryHandler> _logger;

    public FindReservationByVariantIdQueryHandler(
        ILogger<FindReservationByVariantIdQueryHandler> logger,
        IReservationQuery query)
    {
        _query  = query;
        _logger = logger;
    }

    public async Task<IReadOnlyList<VariantReservationDto>> Handle(
        FindReservationByVariantIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}",
            nameof(FindReservationByVariantIdQuery),
            request.VariantId);
        
        var result= await _query.FindByVariantIdAsync(
            variantId: request.VariantId,
            sortBy: request.SortBy,
            page: request.Page,
            pageSize: request.PageSize,
            descending: request.Descending,
            includeExpired: request.IncludeExpired,
            isDeleted: request.IsDeleted,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. VariantId: {VariantId}",
            nameof(FindReservationByVariantIdQuery),
            request.VariantId);

        return result;
    }
}