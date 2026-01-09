/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class ProductPriceHistoryRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductPriceHistoryRepository _productPriceHistoryRepository;
    #region Create Edit Delete
    [Fact]
    public async Task CreateProductPriceHistoryAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arranges
       var priceHistory = new ProductPriceHistoryEntity()
       {
           Id = Guid.NewGuid(),
           Price = 4500,
           OldPrice = 3990,
           DiscountPrice = 1000,
           DiscountPercent = 0,
           StartDate = DateTime.UtcNow.AddHours(3),
           EndDate = null,
           ChangedBy = "seller",
           ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
       };
       // Act
       var result = await _productPriceHistoryRepository.CreateAsync(priceHistory, CancellationToken.None);
       // Assert
       Assert.Equal(priceHistory.Id, result);
       var priceHistoryExists = await _context.PriceHistories
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == priceHistory.Id);
       
       Assert.NotNull(priceHistoryExists);
       Assert.Equal(priceHistory.Id, priceHistoryExists.Id);
       
    }

    [Fact]
    public async Task CreateProductPriceHistoryAsync_FailOnEmpty_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var priceHistory = new ProductPriceHistoryEntity();
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productPriceHistoryRepository
               .CreateAsync(
                   priceHistory, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductPriceHistoryAsync_FailOnWrongProductVariantId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var priceHistory = new ProductPriceHistoryEntity()
       {
           Id = Guid.NewGuid(),
           Price = 4500,
           OldPrice = 3990,
           DiscountPrice = 1000,
           DiscountPercent = 0,
           StartDate = DateTime.UtcNow.AddHours(3),
           EndDate = null,
           ChangedBy = "seller",
           ProductVariantId = Guid.NewGuid()
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productPriceHistoryRepository
               .CreateAsync(
                   priceHistory, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductPriceHistoryAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var priceHistoryExists = await _context.PriceHistories
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.ProductPriceHistoryIdForUpdate);
       
       if (priceHistoryExists is null) 
           Assert.Fail();
       
       //Price = 4500,
       // OldPrice = 3990,
       // DiscountPrice = 1000,
       // DiscountPercent = 0,
       // StartDate = DateTime.UtcNow.AddHours(3),
       // EndDate = null,
       // ChangedBy = "seller",
       // ProductVariantId = Guid.NewGuid()

       priceHistoryExists.Price = 9000;
       priceHistoryExists.OldPrice = 3000;
       priceHistoryExists.DiscountPrice = -6000;
       priceHistoryExists.DiscountPercent = 5252;
       
       await _productPriceHistoryRepository.UpdateAsync(priceHistoryExists, CancellationToken.None);
       // Assert
       var priceHistoryResult = await _context.PriceHistories
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductPriceHistoryIdForUpdate);
       
       Assert.NotNull(priceHistoryResult);
       Assert.Equal(priceHistoryExists.Price, priceHistoryResult.Price);
       Assert.Equal(priceHistoryExists.OldPrice, priceHistoryResult.OldPrice);
       Assert.Equal(priceHistoryExists.DiscountPrice, priceHistoryResult.DiscountPrice);
       Assert.Equal(priceHistoryExists.DiscountPercent, priceHistoryResult.DiscountPercent);
    }

    [Fact]
    public async Task UpdateProductPriceHistoryAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var priceHistory = new ProductPriceHistoryEntity()
       {
           Id = Guid.NewGuid(),
           Price = 4500,
           OldPrice = 3990,
           DiscountPrice = 1000,
           DiscountPercent = 0,
           StartDate = DateTime.UtcNow.AddHours(3),
           EndDate = null,
           ChangedBy = "seller",
           ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _productPriceHistoryRepository.UpdateAsync(
                   priceHistory, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProductPriceHistoryAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Assert
       var priceHistoryExists = await _context.PriceHistories
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductPriceHistoryIdForDelete);
       Assert.NotNull(priceHistoryExists);
       // Act
       await _productPriceHistoryRepository.DeleteAsync(
           TestDataConstants.ProductPriceHistoryIdForDelete, 
           CancellationToken.None);
       // Assert
       var priceHistoryResult = await _context.PriceHistories
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductPriceHistoryIdForDelete);
       Assert.Null(priceHistoryResult);
    }

    [Fact]
    public async Task DeleteProductPriceHistoryAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
      _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productPriceHistoryRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllPriceHistoriesAsync_WithDefaultParameters_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var priceHistories = await _productPriceHistoryRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(priceHistories);
       Assert.Equal(TestDataConstants.OverallProductPriceHistoriesCount, priceHistories.Count());
    }

    [Fact]
    public async Task FindAllProductPriceHistoriesAsync_CountLimit_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var priceHistories = await _productPriceHistoryRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(priceHistories);
       Assert.Equal(2, priceHistories.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllProductPriceHistoriesAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var priceHistories = await _productPriceHistoryRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductPriceHistorySortBy.Id);
       // Assert
       Assert.NotNull(priceHistories);
       Assert.Equal(TestDataConstants.OverallProductPriceHistoriesCount, priceHistories.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllProductPriceHistoriesAsync_SortById_DescendingTrue_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var priceHistories = await _productPriceHistoryRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductPriceHistorySortBy.Id);
       // Assert
       Assert.NotNull(priceHistories);
       Assert.Equal(TestDataConstants.OverallProductPriceHistoriesCount, priceHistories.Count());
       // TODO:
       /*Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
       Assert.Equal(TestDataConstants.SellerIdForDelete, result[3].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting1, result[4].Id);
       Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);#1#
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindProductPriceHistoryByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var priceHistory = await _productPriceHistoryRepository
           .FindByIdAsync(
               TestDataConstants.ProductPriceHistoryIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(priceHistory);
       Assert.Equal(TestDataConstants.ProductPriceHistoryIdForGetting1, priceHistory.Id);
    }

    [Fact]
    public async Task FindProductPriceHistoryByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var priceHistory = await _productPriceHistoryRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(priceHistory);
    }

    [Fact]
    public async Task GetProductPriceHistoryByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var priceHistory = await _productPriceHistoryRepository
           .GetByIdAsync(
               TestDataConstants.ProductPriceHistoryIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(priceHistory);
       Assert.Equal(TestDataConstants.ProductPriceHistoryIdForGetting1, priceHistory.Id);
    }

    [Fact]
    public async Task GetProductPriceHistoryByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productPriceHistoryRepository = new ProductPriceHistoryRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productPriceHistoryRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}*/