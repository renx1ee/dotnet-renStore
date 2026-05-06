using RenStore.Delivery.Application.Abstractions;
using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Requests;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.RegisterWithRussianPost;

internal sealed class RegisterWithRussianPostCommandHandler(
    IDeliveryOrderRepository repository,
    IRussianPostService      russianPostService,
    ILogger<RegisterWithRussianPostCommandHandler> logger)
    : IRequestHandler<RegisterWithRussianPostCommand, string>
{
    public async Task<string> Handle(
        RegisterWithRussianPostCommand request,
        CancellationToken              cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id}",
            nameof(RegisterWithRussianPostCommand),
            request.DeliveryOrderId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), 
                request.DeliveryOrderId);
        
        var result = await russianPostService.CreateShipmentAsync(
            new CreateRussianPostShipmentRequest(
                RecipientName:  request.RecipientName,
                RecipientPhone: request.RecipientPhone,
                AddressTo:      request.AddressTo,
                PostcodeFrom:   request.PostcodeFrom,
                PostcodeTo:     request.PostcodeTo,
                WeightGrams:    request.WeightGrams,
                ValueRub:       request.ValueRub,
                OrderId:        order.OrderId.ToString()),
            cancellationToken);

        if (!result.IsSucceeded)
        {
            throw new InvalidOperationException(
                $"Russian Post rejected shipment: {result.ErrorMessage}");
        }
        
        order.AssignTrackingNumber(result.TrackingNumber, DateTimeOffset.UtcNow);
        
        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder registered with Russian Post. " +
            "DeliveryOrderId={Id} TrackingNumber={Tracking}",
            order.Id, result.TrackingNumber);

        return result.TrackingNumber;
    }
}