using Microsoft.EntityFrameworkCore;
using RenStore.Order.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Order.Persistence.Write.Projections;

internal sealed class OrderProjection 
    : RenStore.Order.Application.Abstractions.Projections.IOrderProjection
{
    private readonly OrderingDbContext _context;

    public OrderProjection(OrderingDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(
        OrderReadModel order,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);

        await _context.Orders.AddAsync(order, cancellationToken);

        return order.Id;
    }
    
    public void Remove(OrderReadModel order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _context.Orders.Remove(order);
    }
    
    private async Task<OrderReadModel> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _context.Orders
            .FirstOrDefaultAsync(x =>
                    x.Id == id,
                cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                name: typeof(OrderReadModel), 
                id);
        }

        return result;
    }
}