using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryTariff.Queries.FindAll;

public sealed record FindAllDeliveryTariffsQuery(bool? IsDeleted = false)
    : IRequest<IReadOnlyList<DeliveryTariffReadModel>>;