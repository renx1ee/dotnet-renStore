using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindById;

internal sealed class FindPaymentByIdQueryHandler(
    IPaymentQuery paymentQuery,
    ILogger<FindPaymentByIdQueryHandler> logger)
    : IRequestHandler<FindPaymentByIdQuery, PaymentReadModel?>
{
    public async Task<PaymentReadModel?> Handle(
        FindPaymentByIdQuery request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Fetching payment by id. PaymentId={PaymentId}",
            request.PaymentId);

        var result = await paymentQuery.FindByIdAsync(request.PaymentId, cancellationToken);

        if (result is null)
            logger.LogWarning(
                "Payment not found. PaymentId={PaymentId}",
                request.PaymentId);

        return result;
    }
}