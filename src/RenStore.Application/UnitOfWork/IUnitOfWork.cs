using RenStore.Domain.Repository;

namespace RenStore.Application.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IProductVariantRepository ProductVariants { get; }
    /*IProductDetailRepository ProductDetails { get; }*/
    IProductPriceHistoryRepository PriceHistories{ get; }
    IProductAttributeRepository ProductAttributes { get; }
    
    Task<int> SaveChangesAsync(CancellationToken calcellationToken);
    Task BeginTransactionAsync(CancellationToken calcellationToken);
    Task CommitAsync(CancellationToken calcellationToken);
    Task RollbackAsync(CancellationToken calcellationToken);
}