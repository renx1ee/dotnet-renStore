using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindAllDeliveryOrders;

public sealed record FindAllDeliveryOrdersQuery(
    DeliveryOrderSortBy SortBy = DeliveryOrderSortBy.CreatedAt,
    uint                Page = 1,
    uint                PageSize = 25,
    bool                Descending = true,
    DeliveryStatus?     Status = null) 
    : IRequest<IReadOnlyList<DeliveryOrderReadModel>>;