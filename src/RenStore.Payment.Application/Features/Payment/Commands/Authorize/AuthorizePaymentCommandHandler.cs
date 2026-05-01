using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.Authorize;

internal sealed class AuthorizePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<AuthorizePaymentCommandHandler> logger)
    : IRequestHandler<AuthorizePaymentCommand>
{
    public async Task Handle(
        AuthorizePaymentCommand request,
        CancellationToken       cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(AuthorizePaymentCommand),
            request.PaymentId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);
        }

        payment.Authorize(
            now:               DateTimeOffset.UtcNow,
            attemptId:         request.AttemptId,
            externalPaymentId: request.ExternalPaymentId,
            externalAuthCode:  request.ExternalAuthCode);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Payment authorized. PaymentId={PaymentId} ExternalId={ExternalId}",
            payment.Id, request.ExternalPaymentId);
    }
}