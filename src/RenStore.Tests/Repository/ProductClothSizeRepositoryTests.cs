/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Clothes;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class ProductClothSizeRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductClothSizeRepository _productClothSizeRepository;
    #region Create Edit Delete
    [Fact]
    public async Task CreateProductClothSizeAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arranges
       var clothSize = new ProductClothSizeEntity()
       {
           Id = Guid.NewGuid(),
           ClothSize = ClothesSizes.XS,
           InStock = 27,
           ProductClothId = TestDataConstants.ProductClothIdForUpdate
       };
       // Act
       var result = await _productClothSizeRepository.CreateAsync(clothSize, CancellationToken.None);
       // Assert
       Assert.Equal(clothSize.Id, result);
       var productSizeExists = await _context.ProductClothSizes
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == clothSize.Id);
       
       Assert.NotNull(productSizeExists);
       Assert.Equal(clothSize.Id, productSizeExists.Id);
       Assert.Equal(clothSize.ClothSize, productSizeExists.ClothSize);
       Assert.Equal(clothSize.InStock, productSizeExists.InStock);
       Assert.Equal(clothSize.ProductClothId, productSizeExists.ProductClothId);
    }

    [Fact]
    public async Task CreateProductClothSizeAsync_FailOnEmpty_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var clothSize = new ProductClothSizeEntity();
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productClothSizeRepository
               .CreateAsync(
                   clothSize, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductClothSizeAsync_FailOnWrongProductClothId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var clothSize = new ProductClothSizeEntity()
       {
           Id = Guid.NewGuid(),
           ClothSize = ClothesSizes.XS,
           InStock = 27,
           ProductClothId = Guid.NewGuid()
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productClothSizeRepository
               .CreateAsync(
                   clothSize, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductClothSizeAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var clothSizeExists = await _context.ProductClothSizes
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.ProductClothSizeIdForUpdate);
       
       if (clothSizeExists is null) 
           Assert.Fail();
       
       clothSizeExists.ClothSize = ClothesSizes.XXXL;
       clothSizeExists.InStock = clothSizeExists.InStock - 1;
       
       await _productClothSizeRepository.UpdateAsync(clothSizeExists, CancellationToken.None);
       // Assert
       var clothSizeResult = await _context.ProductClothSizes
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductClothSizeIdForUpdate);
       
       Assert.NotNull(clothSizeResult);
       Assert.Equal(clothSizeExists.ClothSize, clothSizeResult.ClothSize);
       Assert.Equal(clothSizeExists.InStock, clothSizeResult.InStock);
    }

    [Fact]
    public async Task UpdateProductClothSizeAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var clothSize = new ProductClothSizeEntity()
       {
           Id = Guid.NewGuid(),
           ClothSize = ClothesSizes.XS,
           InStock = 27,
           ProductClothId = TestDataConstants.ProductClothIdForUpdate
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _productClothSizeRepository.UpdateAsync(
                   clothSize, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProductClothSizeAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Assert
       var clothSizeExists = await _context.ProductClothSizes
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductClothSizeIdForDelete);
       Assert.NotNull(clothSizeExists);
       // Act
       await _productClothSizeRepository.DeleteAsync(
           TestDataConstants.ProductClothSizeIdForDelete, 
           CancellationToken.None);
       // Assert
       var clothSizeResult = await _context.ProductClothSizes
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductClothSizeIdForDelete);
       Assert.Null(clothSizeResult);
    }

    [Fact]
    public async Task DeleteProductClothSizeAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productClothSizeRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllProductClothSizesAsync_WithDefaultParameters_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var clothSizes = await _productClothSizeRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(clothSizes);
       Assert.Equal(TestDataConstants.OverallProductClothSizesCount, clothSizes.Count());
    }

    [Fact]
    public async Task FindAllProductClothSizesAsync_CountLimit_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var clothSizes = await _productClothSizeRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(clothSizes);
       Assert.Equal(2, clothSizes.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllProductClothSizesAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var clothSizes = await _productClothSizeRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductClothSizeSortBy.Id);
       // Assert
       Assert.NotNull(clothSizes);
       Assert.Equal(TestDataConstants.OverallProductClothSizesCount, clothSizes.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllProductClothSizesAsync_SortById_DescendingTrue_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var clothSizes = await _productClothSizeRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductClothSizeSortBy.Id);
       // Assert
       Assert.NotNull(clothSizes);
       Assert.Equal(TestDataConstants.OverallProductClothSizesCount, clothSizes.Count());
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
    public async Task FindProductClothSizeByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var clothSize = await _productClothSizeRepository
           .FindByIdAsync(
               TestDataConstants.ProductClothSizeIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(clothSize);
       Assert.Equal(TestDataConstants.ProductClothSizeIdForGetting1, clothSize.Id);
    }

    [Fact]
    public async Task FindProductClothSizeByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var clothSize = await _productClothSizeRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(clothSize);
    }

    [Fact]
    public async Task GetProductClothSizeByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var clothSize = await _productClothSizeRepository
           .GetByIdAsync(
               TestDataConstants.ProductClothSizeIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(clothSize);
       Assert.Equal(TestDataConstants.ProductClothSizeIdForGetting1, clothSize.Id);
    }

    [Fact]
    public async Task GetProductClothSizeByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productClothSizeRepository = new ProductClothSizeRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productClothSizeRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}*/