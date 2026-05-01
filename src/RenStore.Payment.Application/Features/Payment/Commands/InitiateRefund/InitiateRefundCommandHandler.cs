using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.InitiateRefund;

internal sealed class InitiateRefundCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<InitiateRefundCommandHandler> logger)
    : IRequestHandler<InitiateRefundCommand, Guid>
{
    public async Task<Guid> Handle(
        InitiateRefundCommand request,
        CancellationToken     cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId} Amount={Amount}",
            nameof(InitiateRefundCommand),
            request.PaymentId,
            request.Amount);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken)
                      ?? throw new NotFoundException(typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);

        var refundId = payment.InitiateRefund(
            DateTimeOffset.UtcNow,
            request.Amount,
            request.Reason);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Refund initiated. RefundId={RefundId} PaymentId={PaymentId}",
            refundId, payment.Id);

        return refundId;
    }
}