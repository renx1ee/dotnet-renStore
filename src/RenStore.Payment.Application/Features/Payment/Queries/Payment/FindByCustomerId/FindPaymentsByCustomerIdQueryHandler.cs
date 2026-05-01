using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindByCustomerId;

internal sealed class FindPaymentsByCustomerIdQueryHandler(
    IPaymentQuery paymentQuery,
    ILogger<FindPaymentsByCustomerIdQueryHandler> logger)
    : IRequestHandler<FindPaymentsByCustomerIdQuery, IReadOnlyList<PaymentReadModel>>
{
    public async Task<IReadOnlyList<PaymentReadModel>> Handle(
        FindPaymentsByCustomerIdQuery request,
        CancellationToken             cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. CustomerId={CustomerId} Page={Page} PageSize={PageSize} SortBy={SortBy} Status={Status}",
            nameof(FindPaymentsByCustomerIdQuery),
            request.CustomerId,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Status);

        var result = await paymentQuery.FindByCustomerIdAsync(
            customerId:        request.CustomerId,
            sortBy:            request.SortBy,
            page:              request.Page,
            pageSize:          request.PageSize,
            descending:        request.Descending,
            status:            request.Status,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Fetched {Count} payments for customer. CustomerId={CustomerId}",
            result.Count,
            request.CustomerId);

        return result;
    }
}