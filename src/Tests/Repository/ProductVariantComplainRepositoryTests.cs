using Microsoft.EntityFrameworkCore;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using Tests.Common;

namespace Tests.Repository;

public class ProductVariantComplainRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductVariantComplainRepository _productVariantComplainRepository;
    #region Create Update Delete
    
    [Fact]
    public async Task CreateProductVariantComplainAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = new ProductVariantComplainEntity
        {
            Id = Guid.NewGuid(),
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedDate = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting7,
            UserId = TestDataConstants.UserIdForGettingSeller4
        };
        // Act
        await _productVariantComplainRepository.CreateAsync(complain, CancellationToken.None);
        // Assert
        var result = await _context.ProductVariantComplains
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == complain.Id);
        Assert.NotNull(result);
        Assert.Equal(complain.Id, result.Id);
        Assert.Equal(complain.Comment, result.Comment);
        Assert.Equal(complain.CreatedDate, result.CreatedDate);
        Assert.Equal(complain.Status, result.Status);
        Assert.Equal(complain.ProductVariantId, result.ProductVariantId);
        Assert.Equal(complain.UserId, result.UserId);
        Assert.Equal(complain.CustomReason, result.CustomReason);
    }
    
    [Fact]
    public async Task CreateProductVariantComplainAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = new ProductVariantComplainEntity
        {
            Id = Guid.NewGuid(),
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedDate = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting7,
            UserId = Guid.NewGuid().ToString()
        };
        // Act & Assert
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(async () => 
            await _productVariantComplainRepository
                .CreateAsync(
                    complain: complain, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateProductVariantComplainAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = await _context.ProductVariantComplains
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ProductVariantComplainIdForUpdate);
        complain.CustomReason = "fwawafwafw";
        complain.Comment = "totalPrice";
        complain.Status = ProductComplainStatus.InReview;
        // Act 
        await _productVariantComplainRepository.UpdateAsync(complain, CancellationToken.None);
        // Assert
        var result = await _context.ProductVariantComplains
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ProductVariantComplainIdForUpdate);
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.ProductVariantComplainIdForUpdate, result.Id);
        Assert.Equal(complain.CustomReason, result.CustomReason);
        Assert.Equal(complain.Comment, result.Comment);
        Assert.Equal(complain.Status, result.Status);
    }
    
    [Fact]
    public async Task UpdateProductVariantComplainAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = new ProductVariantComplainEntity
        {
            Id = Guid.NewGuid(),
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedDate = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForUpdate,
            UserId = Guid.NewGuid().ToString()
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _productVariantComplainRepository
                .UpdateAsync(
                    complain, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteProductVariantComplainAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complainItem = await _context.ProductVariantComplains
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ProductVariantComplainIdForDelete);
        Assert.NotNull(complainItem);
        // Act
        await _productVariantComplainRepository.DeleteAsync(TestDataConstants.ProductVariantComplainIdForDelete, CancellationToken.None);
        // Assert
        var result = await _context.ProductVariantComplains
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ProductVariantComplainIdForDelete);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteProductVariantComplainAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _productVariantComplainRepository
                .DeleteAsync(
                    id: wrongId, 
                    CancellationToken.None));
    }
    
    #endregion
    #region All
    [Fact]
    public async Task FindAllShoppingProductVariantComplainsAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _productVariantComplainRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(TestDataConstants.OverallProductVariantComplainsCount, complains.Count());
    }
    
    [Fact]
    public async Task FindAllShoppingProductVariantComplainsAsync_CountLimit_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _productVariantComplainRepository
            .FindAllAsync(
                pageCount: 2,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(2, complains.Count());
    }
    
    [Fact]
    public async Task FindAllShoppingProductVariantComplainsAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _productVariantComplainRepository
            .FindAllAsync(
                sortBy: ProductVariantComplainSortBy.Id,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = complains.ToList();
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(TestDataConstants.OverallProductVariantComplainsCount, complains.Count());
    }
    
    [Fact]
    public async Task FindAllShoppingProductVariantComplainsAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _productVariantComplainRepository
            .FindAllAsync(
                sortBy: ProductVariantComplainSortBy.Id,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = complains.ToList();
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(TestDataConstants.OverallProductVariantComplainsCount, complains.Count());
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindProductVariantComplainByIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _productVariantComplainRepository
            .FindByIdAsync(
                TestDataConstants.ProductVariantComplainIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.ProductVariantComplainIdForGetting1, complain.Id);
    }
    
    [Fact]
    public async Task FindProductVariantComplainByIdAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _productVariantComplainRepository
            .FindByIdAsync(
                id: Guid.NewGuid(), 
                CancellationToken.None);
        // Assert
        Assert.Null(complain);
    }
    
    [Fact]
    public async Task GetProductVariantComplainByIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _productVariantComplainRepository
            .GetByIdAsync(
                TestDataConstants.ProductVariantComplainIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.ProductVariantComplainIdForGetting1, complain.Id);
    }
    
    [Fact]
    public async Task GetProductVariantComplainByIdAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _productVariantComplainRepository 
                .GetByIdAsync(
                    Guid.NewGuid(),
                    CancellationToken.None));
    }
    #endregion
    #region By User ID
    [Fact]
    public async Task FindProductVariantComplainsByUserIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _productVariantComplainRepository
            .FindByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        var result = complain.ToList();
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.UserIdForGettingSeller1, result[0].UserId);
    }
    
    [Fact]
    public async Task FindProductVariantComplainsByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        var result = await _productVariantComplainRepository
            .FindByUserIdAsync(
                userId: wrongUserId, 
                CancellationToken.None);
        // Assert
        Assert.Equal(result, new List<ProductVariantComplainEntity?>());
    }
    
    [Fact]
    public async Task GetProductVariantComplainsByUserIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _productVariantComplainRepository
            .GetByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        var result = complain.ToList();
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.UserIdForGettingSeller1, result[0].UserId);
    }
    
    [Fact]
    public async Task GetProductVariantComplainsByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _productVariantComplainRepository = new ProductVariantComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _productVariantComplainRepository
                .GetByUserIdAsync(
                    userId: wrongUserId,
                    CancellationToken.None));
    }
    #endregion
}