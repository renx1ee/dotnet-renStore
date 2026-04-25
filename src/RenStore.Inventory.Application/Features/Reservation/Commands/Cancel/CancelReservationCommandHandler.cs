using RenStore.Inventory.Domain.Aggregates.Reservation;

namespace RenStore.Inventory.Application.Features.Reservation.Commands.Cancel;

internal sealed class CancelReservationCommandHandler
    : IRequestHandler<CancelReservationCommand>
{
    private readonly ILogger<CancelReservationCommandHandler> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IStockRepository _stockRepository;
    
    public CancelReservationCommandHandler(
        ILogger<CancelReservationCommandHandler> logger,
        IReservationRepository reservationRepository,
        IStockRepository stockRepository)
    {
        _logger                = logger;
        _reservationRepository = reservationRepository;
        _stockRepository       = stockRepository;
    }
    
    public async Task Handle(
        CancelReservationCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ReservationId: {ReservationId}",
            nameof(CancelReservationCommand),
            request.ReservationId);

        var reservation = await _reservationRepository.GetAsync(
            reservationId: request.ReservationId,
            cancellationToken: cancellationToken);

        if (reservation is null)
        {
            throw new NotFoundException(
                name: typeof(VariantReservation), request.ReservationId);
        }

        var stock = await _stockRepository.GetAsync(
            stockId: reservation.StockId,
            cancellationToken: cancellationToken);
        
        if (stock is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStock), reservation.StockId);
        }

        var now = DateTimeOffset.UtcNow;
        
        reservation.MarkAsCancel(
            now: now,
            request.Reason);

        stock.ReturnReservation(
            count: reservation.Quantity,
            now: now);
        
        await _stockRepository.SaveAsync(stock, cancellationToken);
        await _reservationRepository.SaveAsync(reservation, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ReservationId: {ReservationId}",
            nameof(CancelReservationCommand),
            request.ReservationId);
    }
}