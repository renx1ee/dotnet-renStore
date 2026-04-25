using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using RenStore.Inventory.Application.Abstractions.Queries;
using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Reservation;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Application.Features.Reservation.Commands.Create;

internal sealed class CreateReservationCommandHandler
    : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly ILogger<CreateReservationCommandHandler> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IStockQuery _stockQuery;
    private readonly ReservationOptions _options;
    
    public CreateReservationCommandHandler(
        ILogger<CreateReservationCommandHandler> logger,
        IReservationRepository reservationRepository,
        IStockRepository stockRepository,
        IStockQuery stockQuery,
        IOptions<ReservationOptions> options)
    {
        _logger                = logger;
        _reservationRepository = reservationRepository;
        _stockQuery            = stockQuery;
        _stockRepository       = stockRepository;
        _options               = options.Value;
    }
    
    public async Task<Guid> Handle(
        CreateReservationCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}, SizeId: {SizeId}, OrderId: {OrderId}",
            nameof(CreateReservationCommand),
            request.VariantId,
            request.SizeId,
            request.OrderId);

        var stockDto = await _stockQuery.FindByVariantIdAsync(
            variantId: request.VariantId,
            sizeId: request.SizeId,
            cancellationToken: cancellationToken);

        if (stockDto is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStockReadModel), request.SizeId);
        }

        var stock = await _stockRepository.GetAsync(stockDto.Id, cancellationToken);
        
        if(stock is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStock), stockDto.Id);
        }
        
        if (!stock.CanDecrease(request.Quantity))
        {
            throw new DomainException(
                $"Not enough stock. Available: {stock.InStock}, requested: {request.Quantity}.");
        }
        
        var now = DateTimeOffset.UtcNow;
        var expiredAt = now.AddMinutes(_options.TtlMinutes);
        
        stock.Decrease(request.Quantity, now);

        var reservation = VariantReservation.Create(
            variantId: stock.VariantId,
            sizeId:    stock.SizeId,
            orderId:   request.OrderId,
            quantity:  request.Quantity,
            expiresAt: expiredAt,
            now:       now);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        await _reservationRepository.SaveAsync(reservation, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handling. VariantId: {VariantId}, SizeId: {SizeId}, OrderId: {OrderId}",
            nameof(CreateReservationCommand),
            request.VariantId,
            request.SizeId,
            request.OrderId);

        return reservation.Id;
    }
}