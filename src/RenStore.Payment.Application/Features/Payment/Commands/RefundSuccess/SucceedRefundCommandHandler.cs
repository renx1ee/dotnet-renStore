using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.RefundSuccess;

internal sealed class SucceedRefundCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<SucceedRefundCommandHandler> logger)
    : IRequestHandler<SucceedRefundCommand>
{
    public async Task Handle(
        SucceedRefundCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId} RefundId={RefundId}",
            nameof(SucceedRefundCommand),
            request.PaymentId,
            request.RefundId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken)
                      ?? throw new NotFoundException(typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);

        payment.SucceedRefund(
            now:              DateTimeOffset.UtcNow,
            refundId:         request.RefundId,
            externalRefundId: request.ExternalRefundId);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Refund succeeded. RefundId={RefundId} PaymentId={PaymentId}",
            request.RefundId, payment.Id);
    }
}