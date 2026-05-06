using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryTariff.Queries.FindById;

public sealed record FindDeliveryTariffByIdQuery(int TariffId)
    : IRequest<DeliveryTariffReadModel?>;