using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Address.Queries.FindById;

public sealed record FindAddressByIdQuery(Guid AddressId) 
    : IRequest<AddressReadModel?>;