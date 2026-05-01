using MediatR;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindAll;

public sealed record FindAllPaymentsQuery(
    PaymentSortBy  SortBy = PaymentSortBy.CreatedAt,
    uint           Page = 1,
    uint           PageSize = 25,
    bool           Descending = true,
    PaymentStatus? Status = null) 
    : IRequest<IReadOnlyList<PaymentReadModel>>;