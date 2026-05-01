using MediatR;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindById;

public sealed record FindPaymentByIdQuery(
    Guid PaymentId) 
    : IRequest<PaymentReadModel?>;