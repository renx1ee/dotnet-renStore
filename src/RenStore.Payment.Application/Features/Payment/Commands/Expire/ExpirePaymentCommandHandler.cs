using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.Expire;

internal sealed class ExpirePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<ExpirePaymentCommandHandler> logger)
    : IRequestHandler<ExpirePaymentCommand>
{
    public async Task Handle(
        ExpirePaymentCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(ExpirePaymentCommand),
            request.PaymentId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken)
                      ?? throw new NotFoundException(typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);

        payment.Expire(DateTimeOffset.UtcNow);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogWarning(
            "Payment expired. PaymentId={PaymentId}",
            payment.Id);
    }
}