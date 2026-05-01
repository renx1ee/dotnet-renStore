using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindAll;

internal sealed class FindAllPaymentsQueryHandler(
    IPaymentQuery paymentQuery,
    ILogger<FindAllPaymentsQueryHandler> logger)
    : IRequestHandler<FindAllPaymentsQuery, IReadOnlyList<PaymentReadModel>>
{
    public async Task<IReadOnlyList<PaymentReadModel>> Handle(
        FindAllPaymentsQuery request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Page={Page} PageSize={PageSize} SortBy={SortBy} Status={Status}",
            nameof(FindAllPaymentsQuery),
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Status);

        var result = await paymentQuery.FindAllAsync(
            sortBy:            request.SortBy,
            page:              request.Page,
            pageSize:          request.PageSize,
            descending:        request.Descending,
            status:            request.Status,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Fetched {Count} payments.",
            result.Count);

        return result;
    }
}