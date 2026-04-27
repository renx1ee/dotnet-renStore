using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Abstractions.Projections;

public interface IOrderProjection
{
    Task CommitAsync(CancellationToken cancellationToken);


    Task<Guid> AddAsync(
        OrderReadModel order,
        CancellationToken cancellationToken);

    void Remove(OrderReadModel order);
}