/*using RenStore.Inventory.Domain.Aggregates.Reservation;

namespace RenStore.Inventory.Application.Services;

public sealed class ReservationService : IReservationService
{
    private readonly IStockRepository _stockRepository;
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(
        IStockRepository stockRepository,
        IReservationRepository reservationRepository)
    {
        _stockRepository       = stockRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<VariantReservation> ReserveAsync(
        Guid stockId,
        Guid orderId,
        int quantity,
        DateTimeOffset now,
        DateTimeOffset expiresAt,
        CancellationToken cancellationToken)
    {
        

        return reservation;
    }

    /*public async Task ConfirmAsync(
        Guid reservationId,
        DateTimeOffset now,
        CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetAsync(reservationId, ct)
            ?? throw new NotFoundException(typeof(VariantReservation), reservationId);

        // Подтверждение — сток уже уменьшен при резервации
        // просто меняем статус резервации и вызываем Sell на стоке
        var stock = await _stockRepository.GetAsync(
            reservation.VariantId, reservation.SizeId, ct)
            ?? throw new NotFoundException(typeof(VariantStock), reservationId);

        reservation.Confirm(now);
        stock.Sell(now, reservation.Quantity); // фиксируем продажу

        await _stockRepository.SaveAsync(stock, ct);
        await _reservationRepository.SaveAsync(reservation, ct);
    }

    public async Task CancelAsync(
        Guid reservationId,
        DateTimeOffset now,
        CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetAsync(reservationId, ct)
            ?? throw new NotFoundException(typeof(VariantReservation), reservationId);

        var stock = await _stockRepository.GetAsync(
            reservation.VariantId, reservation.SizeId, ct)
            ?? throw new NotFoundException(typeof(VariantStock), reservationId);

        reservation.Cancel(now);
        stock.ReturnReservation(reservation.Quantity, now); // возвращаем сток

        await _stockRepository.SaveAsync(stock, ct);
        await _reservationRepository.SaveAsync(reservation, ct);
    }

    public async Task ExpireAsync(
        Guid reservationId,
        DateTimeOffset now,
        CancellationToken ct)
    {
        // Аналогично Cancel — возвращаем сток
        var reservation = await _reservationRepository.GetAsync(reservationId, ct)
            ?? throw new NotFoundException(typeof(VariantReservation), reservationId);

        var stock = await _stockRepository.GetAsync(
            reservation.VariantId, reservation.SizeId, ct)
            ?? throw new NotFoundException(typeof(VariantStock), reservationId);

        reservation.Expire(now);
        stock.ReturnReservation(reservation.Quantity, now);

        await _stockRepository.SaveAsync(stock, ct);
        await _reservationRepository.SaveAsync(reservation, ct);
    }#1#
}*/