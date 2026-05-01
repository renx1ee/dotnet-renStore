using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.Cancel;

internal sealed class CancelPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<CancelPaymentCommandHandler> logger)
    : IRequestHandler<CancelPaymentCommand>
{
    public async Task Handle(
        CancelPaymentCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(CancelPaymentCommand),
            request.PaymentId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken)
                      ?? throw new NotFoundException(typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);

        payment.Cancel(DateTimeOffset.UtcNow, request.Reason);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Payment cancelled. PaymentId={PaymentId}",
            payment.Id);
    }
}