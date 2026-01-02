using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums;
using RenStore.Domain.Enums.Clothes;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class ProductClothRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductClothRepository _clothRepository;
    #region Create Update Delete
    [Fact]
    public async Task CreateProductClothAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arranges
       var productCloth = new ProductClothEntity()
       {
           Id = Guid.NewGuid(),
           Gender = Gender.Unisex,
           Season = Season.YearRound,
           Neckline = Neckline.BoatNeck,
           TheCut = TheCut.Free,
           ProductId = TestDataConstants.ProductIdForGetting7
       };
       // Act
       var result = await _clothRepository.CreateAsync(productCloth, CancellationToken.None);
       // Assert
       Assert.Equal(productCloth.Id, result);
       var productClothExists = await _context.ProductClothes
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == productCloth.Id);
       
       Assert.NotNull(productClothExists);
       Assert.Equal(productCloth.Id, productClothExists.Id);
       Assert.Equal(productCloth.Gender, productClothExists.Gender);
       Assert.Equal(productCloth.Season, productClothExists.Season);
       Assert.Equal(productCloth.Neckline, productClothExists.Neckline);
       Assert.Equal(productCloth.TheCut, productClothExists.TheCut);
       Assert.Equal(productCloth.ProductId, productClothExists.ProductId);
    }

    [Fact]
    public async Task CreateProductClothAsync_FailOnEmpty_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productCloth = new ProductClothEntity();
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _clothRepository
               .CreateAsync(
                   productCloth, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductClothAsync_FailOnWrongProductId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productCloth = new ProductClothEntity()
       {
           Id = TestDataConstants.ProductClothIdForGetting1,
           Gender = Gender.Unisex,
           Season = Season.YearRound,
           Neckline = Neckline.BoatNeck,
           TheCut = TheCut.Free,
           ProductId = Guid.NewGuid()
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _clothRepository
               .CreateAsync(
                   productCloth, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductClothAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productClothExists = await _context.ProductClothes
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.ProductClothIdForUpdate);
       
       if (productClothExists is null) 
           Assert.Fail();
       
       productClothExists.Gender = Gender.Woman;
       productClothExists.Season = Season.YearRound;
       productClothExists.Neckline = Neckline.PoloNeck;
       productClothExists.TheCut = TheCut.Free;
       
       await _clothRepository.UpdateAsync(productClothExists, CancellationToken.None);
       // Assert
       var productClothResult = await _context.ProductClothes
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductClothIdForUpdate);
       
       Assert.NotNull(productClothResult);
       Assert.Equal(productClothExists.Gender, productClothResult.Gender);
       Assert.Equal(productClothExists.Season, productClothResult.Season);
       Assert.Equal(productClothExists.Neckline, productClothResult.Neckline);
       Assert.Equal(productClothExists.TheCut, productClothResult.TheCut);
    }

    [Fact]
    public async Task UpdateProductClothAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productCloth = new ProductClothEntity()
       {
           Id = Guid.NewGuid(),
           Gender = Gender.Unisex,
           Season = Season.YearRound,
           Neckline = Neckline.BoatNeck,
           TheCut = TheCut.Free,
           ProductId = TestDataConstants.ProductIdForGetting1
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _clothRepository.UpdateAsync(
                   productCloth, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProductClothAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
      _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productClothExists = await _context.ProductClothes
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductClothIdForDelete);
       Assert.NotNull(productClothExists);
       // Act
       await _clothRepository.DeleteAsync(
           TestDataConstants.ProductClothIdForDelete, 
           CancellationToken.None);
       // Assert
       var productClothResult = await _context.ProductClothes
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductClothIdForDelete);
       Assert.Null(productClothResult);
    }

    [Fact]
    public async Task DeleteProductClothAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
      _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _clothRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllProductClothesAsync_WithDefaultParameters_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productClothes = await _clothRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(productClothes);
       Assert.Equal(TestDataConstants.OverallProductClothesCount, productClothes.Count());
    }

    [Fact]
    public async Task FindAllProductClothesAsync_CountLimit_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
      _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productClothes = await _clothRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(productClothes);
       Assert.Equal(2, productClothes.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllProductClothesAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
      _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productClothes = await _clothRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductClothSortBy.Id);
       // Assert
       Assert.NotNull(productClothes);
       Assert.Equal(TestDataConstants.OverallProductClothesCount, productClothes.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllProductClothesAsync_SortById_DescendingTrue_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
      _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productClothes = await _clothRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductClothSortBy.Id);
       // Assert
       Assert.NotNull(productClothes);
       Assert.Equal(TestDataConstants.OverallProductClothesCount, productClothes.Count());
       // TODO:
       /*Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
       Assert.Equal(TestDataConstants.SellerIdForDelete, result[3].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting1, result[4].Id);
       Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);*/
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindProductClothesByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var product = await _clothRepository
           .FindByIdAsync(
               TestDataConstants.ProductClothIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(product);
       Assert.Equal(TestDataConstants.ProductClothIdForGetting1, product.Id);
    }

    [Fact]
    public async Task FindProductClothByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var product = await _clothRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(product);
    }

    [Fact]
    public async Task GetProductClothByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var product = await _clothRepository
           .GetByIdAsync(
               TestDataConstants.ProductClothIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(product);
       Assert.Equal(TestDataConstants.ProductClothIdForGetting1, product.Id);
    }

    [Fact]
    public async Task GetProductClothByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _clothRepository = new ProductClothRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _clothRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}