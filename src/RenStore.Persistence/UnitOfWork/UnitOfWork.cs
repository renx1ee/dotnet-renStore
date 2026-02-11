/*using Microsoft.EntityFrameworkCore.Storage;
using RenStore.Application.UnitOfWork;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public IProductRepository Products { get; private set; }
    public IProductVariantRepository ProductVariants { get; private set; }
    public IProductDetailRepository ProductDetails { get; private set; }
    public IProductPriceHistoryRepository PriceHistories { get; private set; }
    public IProductAttributeRepository ProductAttributes { get; private set; }

    private IDbContextTransaction _transaction;

    public UnitOfWork(
        ApplicationDbContext context,
        IProductRepository productRepository,
        IProductVariantRepository productVariantRepository,
        IProductDetailRepository productDetailsRepository,
        IProductPriceHistoryRepository priceHistoryRepository,
        IProductAttributeRepository productAttributeRepository)
    {
        this._context = context;
        this.Products = productRepository;
        this.ProductVariants = productVariantRepository;
        this.ProductDetails = productDetailsRepository;
        this.PriceHistories = priceHistoryRepository;
        this.ProductAttributes = productAttributeRepository;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken calcellationToken) =>
        await _context.SaveChangesAsync(calcellationToken);
    
    public async Task BeginTransactionAsync(CancellationToken calcellationToken) =>
        _transaction = await _context.Database.BeginTransactionAsync(calcellationToken);
    
    public async Task CommitAsync(CancellationToken calcellationToken) =>
        await _transaction.CommitAsync(calcellationToken);
    
    public async Task RollbackAsync(CancellationToken calcellationToken) =>
        await _transaction.RollbackAsync(calcellationToken);
    
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}*/