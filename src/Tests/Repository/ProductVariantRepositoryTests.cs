using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using Tests.Common;

namespace Tests.Repository;

public class ProductVariantRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductVariantRepository _productVariantRepository;
    #region Create Update Delete
    [Fact]
    public async Task CreateProductVariantAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arranges
       var productVariant = new ProductVariantEntity()
       {
           Id = Guid.NewGuid(),
           Name = "Sample product",
           NormalizedName = "Sample product".ToUpper(),
           Rating = 3,
           Article = 098765432,
           InStock = 86,
           IsAvailable = true,
           CreatedDate = DateTime.UtcNow,
           Url = "",
           ProductId = TestDataConstants.ProductIdForGetting1,
           ColorId = TestDataConstants.ColorIdForGetting1
       };
       // Act
       var result = await _productVariantRepository.CreateAsync(productVariant, CancellationToken.None);
       // Assert
       Assert.Equal(productVariant.Id, result);
       var productVariantExists = await _context.ProductVariants
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == productVariant.Id);
       
       Assert.NotNull(productVariantExists);
       Assert.Equal(productVariant.Id, productVariantExists.Id);
       Assert.Equal(productVariant.Name, productVariantExists.Name);
       Assert.Equal(productVariant.NormalizedName, productVariantExists.NormalizedName);
       Assert.Equal(productVariant.Rating, productVariantExists.Rating);
       Assert.Equal(productVariant.Article, productVariantExists.Article);
       Assert.Equal(productVariant.InStock, productVariantExists.InStock);
       Assert.Equal(productVariant.IsAvailable, productVariantExists.IsAvailable);
       Assert.Equal(productVariant.CreatedDate, productVariantExists.CreatedDate);
       Assert.Equal(productVariant.Url, productVariantExists.Url);
       Assert.Equal(productVariant.ProductId, productVariantExists.ProductId);
       Assert.Equal(productVariant.ColorId, productVariantExists.ColorId);
    }

    [Fact]
    public async Task CreateProductVariantAsync_FailOnEmpty_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var productVariant = new ProductVariantEntity()
       {
           Article = 9525
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productVariantRepository
               .CreateAsync(
                   productVariant, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductVariantAsync_FailOnWrongProductId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var productVariant = new ProductVariantEntity()
       {
           Id = Guid.NewGuid(),
           Name = "Sample product",
           NormalizedName = "Sample product".ToUpper(),
           Rating = 3,
           Article = 098765432,
           InStock = 86,
           IsAvailable = true,
           CreatedDate = DateTime.UtcNow,
           Url = "",
           ProductId = Guid.NewGuid(),
           ColorId = TestDataConstants.ColorIdForGetting1
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productVariantRepository
               .CreateAsync(
                   productVariant, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductVariantAsync_FailOnWrongColorId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var productVariant = new ProductVariantEntity()
       {
           Id = Guid.NewGuid(),
           Name = "Sample product",
           NormalizedName = "Sample product".ToUpper(),
           Rating = 3,
           Article = 098765432,
           InStock = 86,
           IsAvailable = true,
           CreatedDate = DateTime.UtcNow,
           Url = "",
           ProductId = TestDataConstants.ProductIdForGetting1,
           ColorId = 63723643
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productVariantRepository
               .CreateAsync(
                   productVariant, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductVariantAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var productVariantExists = await _context.ProductVariants
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.ProductVariantIdForUpdate);
       
       if (productVariantExists is null) 
           Assert.Fail();
       
       productVariantExists.Name = "Updated Name";
       productVariantExists.NormalizedName = "Updated Name".ToUpper();
       productVariantExists.InStock = productVariantExists.InStock - 1;
       productVariantExists.Article = 021551;
       productVariantExists.Rating = 5;
       productVariantExists.IsAvailable = false;
       
       await _productVariantRepository.UpdateAsync(productVariantExists, CancellationToken.None);
       // Assert
       var productVariantResult = await _context.ProductVariants
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductVariantIdForUpdate);
       
       Assert.NotNull(productVariantResult);
       Assert.Equal(productVariantExists.Name, productVariantResult.Name);
       Assert.Equal(productVariantExists.NormalizedName, productVariantResult.NormalizedName);
       Assert.Equal(productVariantExists.InStock, productVariantResult.InStock);
       Assert.Equal(productVariantExists.Article, productVariantResult.Article);
       Assert.Equal(productVariantExists.Rating, productVariantResult.Rating);
       Assert.Equal(productVariantExists.IsAvailable, productVariantResult.IsAvailable);
    }

    [Fact]
    public async Task UpdateProductVariantAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariant = new ProductVariantEntity()
       {
           Id = Guid.NewGuid(),
           Name = "Sample product",
           NormalizedName = "Sample product".ToUpper(),
           Rating = 3,
           Article = 098765432,
           InStock = 86,
           IsAvailable = true,
           CreatedDate = DateTime.UtcNow,
           Url = "",
           ProductId = Guid.NewGuid(),
           ColorId = TestDataConstants.ColorIdForGetting1
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _productVariantRepository.UpdateAsync(
                   productVariant, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProductVariantAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Assert
       var productVariantExists = await _context.ProductVariants
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductVariantIdForDelete);
       Assert.NotNull(productVariantExists);
       // Act
       await _productVariantRepository.DeleteAsync(
           TestDataConstants.ProductVariantIdForDelete, 
           CancellationToken.None);
       // Assert
       var productVariantResult = await _context.ProductVariants
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductVariantIdForDelete);
       Assert.Null(productVariantResult);
    }

    [Fact]
    public async Task DeleteProductVariantAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productVariantRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllProductVariantsAsync_WithDefaultParameters_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariants = await _productVariantRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(productVariants);
       Assert.Equal(TestDataConstants.OverallProductVariantCount, productVariants.Count());
    }

    [Fact]
    public async Task FindAllProductVariantsAsync_CountLimit_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariants = await _productVariantRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(productVariants);
       Assert.Equal(2, productVariants.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllProductVariantsAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariants = await _productVariantRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductVariantSortBy.Id);
       // Assert
       Assert.NotNull(productVariants);
       Assert.Equal(TestDataConstants.OverallProductVariantCount, productVariants.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllProductVariantsAsync_SortById_DescendingTrue_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariants = await _productVariantRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductVariantSortBy.Id);
       // Assert
       Assert.NotNull(productVariants);
       Assert.Equal(TestDataConstants.OverallProductVariantCount, productVariants.Count());
       // TODO:
       /*Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
       Assert.Equal(TestDataConstants.SellerIdForDelete, result[3].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting1, result[4].Id);
       Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);*/
    }

    [Fact]
    public async Task FindAllProductVariantsAsync_WithIsAvailableTrue_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariants = await _productVariantRepository
           .FindAllAsync(
               isAvailable: true,
               cancellationToken: CancellationToken.None);
       // Assert
       Assert.NotNull(productVariants);
       Assert.Equal(TestDataConstants.OverallProductVariantCount - 2, productVariants.Count());
    }

    [Fact]
    public async Task FindAllProductVariantsAsync_WithIsBlockedFalse_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariants = await _productVariantRepository
           .FindAllAsync(
               isAvailable: false,
               cancellationToken: CancellationToken.None);
       // Assert
       Assert.NotNull(productVariants);
       Assert.Equal(2, productVariants.Count());
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindProductVariantByIdAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariant = await _productVariantRepository
           .FindByIdAsync(
               TestDataConstants.ProductVariantIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productVariant);
       Assert.Equal(TestDataConstants.ProductVariantIdForGetting1, productVariant.Id);
    }

    [Fact]
    public async Task FindProductVariantByIdAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var productVariant = await _productVariantRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(productVariant);
    }

    [Fact]
    public async Task GetProductVariantByIdAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productVariant = await _productVariantRepository
           .GetByIdAsync(
               TestDataConstants.ProductVariantIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productVariant);
       Assert.Equal(TestDataConstants.ProductVariantIdForGetting1, productVariant.Id);
    }

    [Fact]
    public async Task GetProductVariantByIdAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productVariantRepository = new ProductVariantRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productVariantRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}