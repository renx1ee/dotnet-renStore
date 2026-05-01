using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.RefundFail;

internal sealed class FailRefundCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<FailRefundCommandHandler> logger)
    : IRequestHandler<FailRefundCommand>
{
    public async Task Handle(
        FailRefundCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId} RefundId={RefundId}",
            nameof(FailRefundCommand),
            request.PaymentId,
            request.RefundId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken)
                      ?? throw new NotFoundException(typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);

        payment.FailRefund(
            now:      DateTimeOffset.UtcNow,
            refundId: request.RefundId,
            reason:   request.Reason);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogWarning(
            "Refund failed. RefundId={RefundId} PaymentId={PaymentId} Reason={Reason}",
            request.RefundId, payment.Id, request.Reason);
    }
}