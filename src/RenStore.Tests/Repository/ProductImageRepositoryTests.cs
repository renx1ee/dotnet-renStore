/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class ProductImageRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductImageRepository _productImageRepository;
    #region Create Edit Delete
    [Fact]
    public async Task CreateProductImageAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arranges
       var productImage = new ProductImage()
       {
           Id = Guid.NewGuid(),
           OriginalFileName = Guid.NewGuid().ToString(),
           StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
           FileSizeBytes = 500,
           IsMain = false,
           SortOrder = 1,
           UploadedAt = DateTime.UtcNow,
           Weight = 500,
           Height = 500,
           ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
       };
       // Act
       var result = await _productImageRepository.CreateAsync(productImage, CancellationToken.None);
       // Assert
       Assert.Equal(productImage.Id, result);
       var productImageExists = await _context.ProductImages
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == productImage.Id);
       
       Assert.NotNull(productImageExists);
       Assert.Equal(productImage.Id, productImageExists.Id);

    }

    [Fact]
    public async Task CreateProductImageAsync_FailOnEmpty_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productImage = new ProductImage();
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productImageRepository
               .CreateAsync(
                   productImage, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductImageAsync_FailOnWrongProductId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productImage = new ProductImage()
       {
           Id = Guid.NewGuid(),
           OriginalFileName = Guid.NewGuid().ToString(),
           StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
           FileSizeBytes = 500,
           IsMain = false,
           SortOrder = 1,
           UploadedAt = DateTime.UtcNow,
           Weight = 500,
           Height = 500,
           ProductVariantId = Guid.NewGuid()
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productImageRepository
               .CreateAsync(
                   productImage, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductImageAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productImageExists = await _context.ProductImages
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.ProductImageIdForUpdate);
       
       if (productImageExists is null) 
           Assert.Fail();
       
       productImageExists.OriginalFileName = "Updated Key";
       productImageExists.StoragePath = Guid.NewGuid().ToString();
       productImageExists.FileSizeBytes = 400;
       productImageExists.IsMain = false;
       productImageExists.SortOrder = 3;
       productImageExists.Weight = 600;
       productImageExists.Height = 700;
       
       await _productImageRepository.UpdateAsync(productImageExists, CancellationToken.None);
       // Assert
       var productImageResult = await _context.ProductImages
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductImageIdForUpdate);
       
       Assert.NotNull(productImageResult);
       Assert.Equal(productImageExists.OriginalFileName, productImageResult.OriginalFileName);
       Assert.Equal(productImageExists.StoragePath, productImageResult.StoragePath);
       Assert.Equal(productImageExists.FileSizeBytes, productImageResult.FileSizeBytes);
       Assert.Equal(productImageExists.IsMain, productImageResult.IsMain);
       Assert.Equal(productImageExists.SortOrder, productImageResult.SortOrder);
       Assert.Equal(productImageExists.Weight, productImageResult.Weight);
       Assert.Equal(productImageExists.Height, productImageResult.Height);
    }

    [Fact]
    public async Task UpdateProductImageAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productImage = new ProductImage()
       {
           Id = Guid.NewGuid(),
           OriginalFileName = Guid.NewGuid().ToString(),
           StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
           FileSizeBytes = 500,
           IsMain = false,
           SortOrder = 1,
           UploadedAt = DateTime.UtcNow,
           Weight = 500,
           Height = 500,
           ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _productImageRepository.UpdateAsync(
                   productImage, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProductImageAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Assert
       var productImageExists = await _context.ProductImages
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductImageIdForDelete);
       Assert.NotNull(productImageExists);
       // Act
       await _productImageRepository.DeleteAsync(
           TestDataConstants.ProductImageIdForDelete, 
           CancellationToken.None);
       // Assert
       var productImageResult = await _context.ProductImages
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductImageIdForDelete);
       Assert.Null(productImageResult);
    }

    [Fact]
    public async Task DeleteProductImageAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productImageRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllProductImagesAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productImages = await _productImageRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(productImages);
       Assert.Equal(TestDataConstants.OverallProductImagesCount, productImages.Count());
    }

    [Fact]
    public async Task FindAllProductImagesAsync_CountLimit_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productImages = await _productImageRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(productImages);
       Assert.Equal(2, productImages.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllProductImagesAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productImages = await _productImageRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductImageSortBy.Id);
       // Assert
       Assert.NotNull(productImages);
       Assert.Equal(TestDataConstants.OverallProductImagesCount, productImages.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllProductImagesAsync_SortById_DescendingTrue_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productImages = await _productImageRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductImageSortBy.Id);
       // Assert
       Assert.NotNull(productImages);
       Assert.Equal(TestDataConstants.OverallProductImagesCount, productImages.Count());
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
    public async Task FindProductImageByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productImage = await _productImageRepository
           .FindByIdAsync(
               TestDataConstants.ProductImageIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productImage);
       Assert.Equal(TestDataConstants.ProductImageIdForGetting1, productImage.Id);
    }

    [Fact]
    public async Task FindProductImageByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var productImage = await _productImageRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(productImage);
    }

    [Fact]
    public async Task GetProductImageByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productImage = await _productImageRepository
           .GetByIdAsync(
               TestDataConstants.ProductImageIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productImage);
       Assert.Equal(TestDataConstants.ProductImageIdForGetting1, productImage.Id);
    }

    [Fact]
    public async Task GetProductImageByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _productImageRepository = new ProductImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productImageRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}*/