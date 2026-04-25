using RenStore.Inventory.Domain.Aggregates.Reservation;

namespace RenStore.Inventory.Application.Features.Reservation.Commands.Confirm;

internal sealed class ConfirmReservationCommandHandler
    : IRequestHandler<ConfirmReservationCommand>
{
    private readonly ILogger<ConfirmReservationCommandHandler> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IStockRepository _stockRepository;
    
    public ConfirmReservationCommandHandler(
        ILogger<ConfirmReservationCommandHandler> logger,
        IReservationRepository reservationRepository,
        IStockRepository stockRepository)
    {
        _logger                = logger;
        _reservationRepository = reservationRepository;
        _stockRepository       = stockRepository;
    }
    
    public async Task Handle(
        ConfirmReservationCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ReservationId: {ReservationId}",
            nameof(ConfirmReservationCommand),
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
        
        reservation.MarkAsConfirmed(now);

        stock.Sell(now, reservation.Quantity);
        
        await _stockRepository.SaveAsync(stock, cancellationToken);
        await _reservationRepository.SaveAsync(reservation, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ReservationId: {ReservationId}",
            nameof(ConfirmReservationCommand),
            request.ReservationId);
    }
}