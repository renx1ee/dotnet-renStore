using RenStore.Inventory.Domain.Aggregates.Reservation;

namespace RenStore.Inventory.Application.Features.Reservation.Commands.SoftDelete;

internal sealed class SoftDeleteReservationCommandHandler
    : IRequestHandler<SoftDeleteReservationCommand>
{
    private readonly ILogger<SoftDeleteReservationCommandHandler> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly ICurrentUserService _userService;

    public SoftDeleteReservationCommandHandler(
        ILogger<SoftDeleteReservationCommandHandler> logger,
        IReservationRepository reservationRepository,
        ICurrentUserService userService)
    {
        _logger                = logger;
        _reservationRepository = reservationRepository;
        _userService           = userService;
    }

    public async Task Handle(
        SoftDeleteReservationCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ReservationId: {ReservationId}",
            nameof(SoftDeleteReservationCommand),
            request.ReservationId);

        var reservation = await _reservationRepository.GetAsync(
            reservationId: request.ReservationId,
            cancellationToken: cancellationToken);

        if (reservation is null)
        {
            throw new NotFoundException(
                name: typeof(VariantReservation), request.ReservationId);
        }

        var now = DateTimeOffset.UtcNow;

        reservation.Delete(
            updatedById:   _userService.UserId ?? throw new UnauthorizedException(),
            updatedByRole: _userService.Role,
            now: now);

        await _reservationRepository.SaveAsync(reservation, cancellationToken);

        _logger.LogInformation(
            "{Command} handled. ReservationId: {ReservationId}",
            nameof(SoftDeleteReservationCommand),
            request.ReservationId);
    }
}