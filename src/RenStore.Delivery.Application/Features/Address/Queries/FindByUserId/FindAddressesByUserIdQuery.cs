using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Address.Queries.FindByUserId;

public sealed record FindAddressesByUserIdQuery(
    Guid  UserId,
    bool? IsDeleted = false) 
    : IRequest<IReadOnlyList<AddressReadModel>>;