using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.Capture;

internal sealed class CapturePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<CapturePaymentCommandHandler> logger)
    : IRequestHandler<CapturePaymentCommand>
{
    public async Task Handle(
        CapturePaymentCommand request,
        CancellationToken     cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(CapturePaymentCommand),
            request.PaymentId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken);
                      
        if(payment is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Payment.Payment),
                request.PaymentId);
        }

        payment.Capture(DateTimeOffset.UtcNow, request.ExternalPaymentId);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Payment captured. PaymentId={PaymentId}",
            payment.Id);
    }
}