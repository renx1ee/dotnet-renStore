using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Attempts;

internal sealed class FindAttemptsByPaymentIdQueryHandler(
    IPaymentAttemptQuery attemptQuery,
    ILogger<FindAttemptsByPaymentIdQueryHandler> logger)
    : IRequestHandler<FindAttemptsByPaymentIdQuery, IReadOnlyList<PaymentAttemptReadModel>>
{
    public async Task<IReadOnlyList<PaymentAttemptReadModel>> Handle(
        FindAttemptsByPaymentIdQuery request,
        CancellationToken            cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(FindAttemptsByPaymentIdQuery),
            request.PaymentId);

        var result = await attemptQuery.FindByPaymentIdAsync(
            request.PaymentId,
            cancellationToken);

        logger.LogInformation(
            "Fetched {Count} attempts. PaymentId={PaymentId}",
            result.Count, request.PaymentId);

        return result;
    }
}