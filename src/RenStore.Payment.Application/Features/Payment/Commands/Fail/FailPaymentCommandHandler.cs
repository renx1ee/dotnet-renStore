using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.Fail;

internal sealed class FailPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<FailPaymentCommandHandler> logger)
    : IRequestHandler<FailPaymentCommand>
{
    public async Task Handle(
        FailPaymentCommand request,
        CancellationToken  cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId} Reason={Reason}",
            nameof(FailPaymentCommand),
            request.PaymentId,
            request.Reason);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Payment.Payment), 
                request.PaymentId);
        }

        payment.Fail(
            now:               DateTimeOffset.UtcNow,
            attemptId:         request.AttemptId,
            reason:            request.Reason,
            providerErrorCode: request.ProviderErrorCode);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogWarning(
            "Payment failed. PaymentId={PaymentId} Reason={Reason}",
            payment.Id, request.Reason);
    }
}