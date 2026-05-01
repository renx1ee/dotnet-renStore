using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.CreateAttempt;

internal sealed class CreatePaymentAttemptCommandHandler(
    IPaymentRepository paymentRepository,
    ILogger<CreatePaymentAttemptCommandHandler> logger)
    : IRequestHandler<CreatePaymentAttemptCommand, Guid>
{
    public async Task<Guid> Handle(
        CreatePaymentAttemptCommand request,
        CancellationToken           cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(CreatePaymentAttemptCommand),
            request.PaymentId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);
        }

        var attemptId = payment.CreateAttempt(DateTimeOffset.UtcNow);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Payment attempt created. AttemptId={AttemptId} PaymentId={PaymentId}",
            attemptId, payment.Id);

        return attemptId;
    }
}