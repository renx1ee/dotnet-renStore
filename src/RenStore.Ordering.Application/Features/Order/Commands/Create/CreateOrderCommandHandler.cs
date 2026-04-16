using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Domain.Interfaces;

namespace RenStore.Order.Application.Features.Order.Commands.Create;

internal sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    
    public CreateOrderCommandHandler(
        ILogger<CreateOrderCommandHandler> logger,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task<Guid> Handle(
        CreateOrderCommand request, 
        CancellationToken cancellationToken) 
    {
        _logger.LogInformation(
            "Handling {Command} with CustomerId: {CustomerId}",
            nameof(CreateOrderCommand),
            request.CustomerId);
        
        var now = DateTimeOffset.UtcNow;

        var order = RenStore.Order.Domain.Aggregates.Order.Order.Create(
            now: now,
            customerId: request.CustomerId,
            shippingAddress: request.ShippingAddress);
        
        order.AddItem(
            now:                 now,
            variantId:           request.VariantId,
            sizeId:              request.SizeId,
            quantity:            request.Quantity,
            priceAmount:         request.PriceAmount,
            currency:            request.Currency,
            productNameSnapshot: request.ProductNameSnapshot);

        await _orderRepository.SaveAsync(order, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CustomerId: {CustomerId}",
            nameof(CreateOrderCommand),
            request.CustomerId);

        return order.Id;
    }
}