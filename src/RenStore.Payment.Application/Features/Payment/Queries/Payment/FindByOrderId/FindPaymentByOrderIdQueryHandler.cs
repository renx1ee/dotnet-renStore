using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindByOrderId;

internal sealed class FindPaymentByOrderIdQueryHandler(
    IPaymentQuery paymentQuery,
    ILogger<FindPaymentByOrderIdQueryHandler> logger)
    : IRequestHandler<FindPaymentByOrderIdQuery, PaymentReadModel?>
{
    public async Task<PaymentReadModel?> Handle(
        FindPaymentByOrderIdQuery request,
        CancellationToken         cancellationToken)
    {
        logger.LogInformation(
            "Fetching payment by orderId. OrderId={OrderId}",
            request.OrderId);

        return await paymentQuery.FindByOrderIdAsync(request.OrderId, cancellationToken);
    }
}