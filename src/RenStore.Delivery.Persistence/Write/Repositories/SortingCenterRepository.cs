using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class SortingCenterRepository
    (ApplicationDbContext context)
    : RenStore.Delivery.Domain.Interfaces.ISortingCenterRepository
{
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentOutOfRangeException(nameof(context));
    
    public async Task<long> CreateAsync(
        SortingCenter sortingCenter,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sortingCenter);

        var result = await this._context.SortingCenters.AddAsync(sortingCenter, cancellationToken);

        return result.Entity.Id;
    }

    public async Task CreateRangeAsync(
        IReadOnlyCollection<SortingCenter> sortingCenters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sortingCenters);

        var sortingCentersList = sortingCenters as IList<SortingCenter> ?? sortingCenters.ToList();

        if (sortingCentersList.Count == 0) return;

        await this._context.SortingCenters.AddRangeAsync(sortingCentersList, cancellationToken);
    }

    public void Remove(SortingCenter sortingCenter)
    {
        ArgumentNullException.ThrowIfNull(sortingCenter);

        this._context.SortingCenters.Remove(sortingCenter);
    }
    
    public void RemoveRange(IReadOnlyCollection<SortingCenter> sortingCenters)
    {
        ArgumentNullException.ThrowIfNull(sortingCenters);
        
        this._context.SortingCenters.RemoveRange(sortingCenters);
    }
}