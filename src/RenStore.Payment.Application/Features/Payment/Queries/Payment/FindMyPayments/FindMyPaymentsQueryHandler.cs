using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Application.Abstractions.Services;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindMyPayments;

internal sealed class FindMyPaymentsQueryHandler(
    IPaymentQuery        paymentQuery,
    ICurrentUserService  currentUser,
    ILogger<FindMyPaymentsQueryHandler> logger)
    : IRequestHandler<FindMyPaymentsQuery, IReadOnlyList<PaymentReadModel>>
{
    public async Task<IReadOnlyList<PaymentReadModel>> Handle(
        FindMyPaymentsQuery request,
        CancellationToken   cancellationToken)
    {
        var userId = currentUser.UserId;

        logger.LogInformation(
            "Fetching payments for current user. UserId={UserId}",
            userId);

        var result = await paymentQuery.FindByCustomerIdAsync(
            customerId:        userId,
            sortBy:            request.SortBy,
            page:              request.Page,
            pageSize:          request.PageSize,
            descending:        request.Descending,
            status:            request.Status,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Fetched {Count} payments for user. UserId={UserId}",
            result.Count, userId);

        return result;
    }
}