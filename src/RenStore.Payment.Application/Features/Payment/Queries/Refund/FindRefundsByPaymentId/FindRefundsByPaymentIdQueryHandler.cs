using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Refund.FindRefundsByPaymentId;

internal sealed class FindRefundsByPaymentIdQueryHandler(
    IRefundQuery refundQuery,
    ILogger<FindRefundsByPaymentIdQueryHandler> logger)
    : IRequestHandler<FindRefundsByPaymentIdQuery, IReadOnlyList<RefundReadModel>>
{
    public async Task<IReadOnlyList<RefundReadModel>> Handle(
        FindRefundsByPaymentIdQuery request,
        CancellationToken           cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(FindRefundsByPaymentIdQuery),
            request.PaymentId);

        var result = await refundQuery.FindByPaymentIdAsync(
            request.PaymentId,
            cancellationToken);

        logger.LogInformation(
            "Fetched {Count} refunds. PaymentId={PaymentId}",
            result.Count, request.PaymentId);

        return result;
    }
}