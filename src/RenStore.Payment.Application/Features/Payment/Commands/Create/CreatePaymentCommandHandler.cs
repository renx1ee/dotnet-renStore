using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;

namespace RenStore.Payment.Application.Features.Payment.Commands.Create;

internal sealed class CreatePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<CreatePaymentCommandHandler> logger)
    : IRequestHandler<CreatePaymentCommand, Guid>
{
    public async Task<Guid> Handle(
        CreatePaymentCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command} with OrderId: {OrderId}, CustomerId: {CustomerId}",
            nameof(CreatePaymentCommand),
            request.OrderId,
            request.CustomerId);
        
        var payment = Domain.Aggregates.Payment.Payment.Create(
            now:           DateTimeOffset.UtcNow,
            orderId:       request.OrderId,
            customerId:    request.CustomerId,
            amount:        request.Amount,
            currency:      request.Currency,
            provider:      request.Provider,
            paymentMethod: request.PaymentMethod);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Payment created. PaymentId={PaymentId} OrderId={OrderId} Amount={Amount}",
            payment.Id, payment.OrderId, payment.Amount);

        return payment.Id;
    }
}