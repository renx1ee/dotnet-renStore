using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class SellerImageRepositoryTests
{
    private ApplicationDbContext _context;
    private SellerImageRepository _sellerImageRepository;
    #region Create Update Delete
    [Fact]
    public async Task CreateSellerVariantAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arranges
       var sellerImage = new SellerImageEntity()
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
           SellerId = TestDataConstants.SellerIdForGetting1
       };
       // Act
       var result = await _sellerImageRepository.CreateAsync(sellerImage, CancellationToken.None);
       // Assert
       Assert.Equal(sellerImage.Id, result);
       var SellerImageExists = await _context.SellerImages
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == sellerImage.Id);
       
       Assert.NotNull(SellerImageExists);
       Assert.Equal(sellerImage.Id, SellerImageExists.Id);

    }

    [Fact]
    public async Task CreateSellerImageAsync_FailOnEmpty_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext(); 
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var sellerImage = new SellerImageEntity();
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _sellerImageRepository
               .CreateAsync(
                   sellerImage, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateSellerImageAsync_FailOnWrongSellerId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var sellerImage = new SellerImageEntity()
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
           SellerId = 8932859382
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _sellerImageRepository
               .CreateAsync(
                   sellerImage, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateSellerImageAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var sellerImageExists = await _context.SellerImages
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.SellerImageIdForUpdate);
       
       if (sellerImageExists is null) 
           Assert.Fail();
       
       sellerImageExists.OriginalFileName = "Updated Name";
       sellerImageExists.StoragePath = Guid.NewGuid().ToString();
       sellerImageExists.FileSizeBytes = 400;
       sellerImageExists.IsMain = false;
       sellerImageExists.SortOrder = 3;
       sellerImageExists.Weight = 600;
       sellerImageExists.Height = 700;
       
       await _sellerImageRepository.UpdateAsync(sellerImageExists, CancellationToken.None);
       // Assert
       var sellerImageResult = await _context.SellerImages
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.SellerImageIdForUpdate);
       
       Assert.NotNull(sellerImageResult);
       Assert.Equal(sellerImageExists.OriginalFileName, sellerImageResult.OriginalFileName);
       Assert.Equal(sellerImageExists.StoragePath, sellerImageResult.StoragePath);
       Assert.Equal(sellerImageExists.FileSizeBytes, sellerImageResult.FileSizeBytes);
       Assert.Equal(sellerImageExists.IsMain, sellerImageResult.IsMain);
       Assert.Equal(sellerImageExists.SortOrder, sellerImageResult.SortOrder);
       Assert.Equal(sellerImageExists.Weight, sellerImageResult.Weight);
       Assert.Equal(sellerImageExists.Height, sellerImageResult.Height);
    }

    [Fact]
    public async Task UpdateSellerImageAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var sellerImage = new SellerImageEntity()
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
           SellerId = TestDataConstants.SellerIdForGetting1
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _sellerImageRepository.UpdateAsync(
                   sellerImage, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteSellerImageAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Assert
       var sellerImageExists = await _context.SellerImages
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.SellerImageIdForDelete);
       Assert.NotNull(sellerImageExists);
       // Act
       await _sellerImageRepository.DeleteAsync(
           TestDataConstants.SellerImageIdForDelete, 
           CancellationToken.None);
       // Assert
       var sellerImageResult = await _context.SellerImages
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.SellerImageIdForDelete);
       Assert.Null(sellerImageResult);
    }
    
    [Fact]
    public async Task DeleteSellerImageAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _sellerImageRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllSellerImagesAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var sellerImages = await _sellerImageRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(sellerImages);
       Assert.Equal(TestDataConstants.OverallSellerImagesCount, sellerImages.Count());
    }

    [Fact]
    public async Task FindAllSellerImagesAsync_CountLimit_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var sellerImages = await _sellerImageRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(sellerImages);
       Assert.Equal(2, sellerImages.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllSellerImagesAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var sellerImages = await _sellerImageRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: SellerImageSortBy.Id);
       // Assert
       Assert.NotNull(sellerImages);
       Assert.Equal(TestDataConstants.OverallSellerImagesCount, sellerImages.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllSellerImagesAsync_SortById_DescendingTrue_Success_Test()
    { 
        _context = TestDatabaseFixture.CreateReadyContext();
       _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var sellerImages = await _sellerImageRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: SellerImageSortBy.Id);
       // Assert
       Assert.NotNull(sellerImages);
       Assert.Equal(TestDataConstants.OverallSellerImagesCount, sellerImages.Count());
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
    public async Task FindSellerImageByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var sellerImage = await _sellerImageRepository
           .FindByIdAsync(
               TestDataConstants.SellerImageIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(sellerImage);
       Assert.Equal(TestDataConstants.SellerImageIdForGetting1, sellerImage.Id);
    }

    [Fact]
    public async Task FindSellerImageByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var sellerImage = await _sellerImageRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(sellerImage);
    }

    [Fact]
    public async Task GetSellerImageByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var sellerImage = await _sellerImageRepository
           .GetByIdAsync(
               TestDataConstants.SellerImageIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(sellerImage);
       Assert.Equal(TestDataConstants.SellerImageIdForGetting1, sellerImage.Id);
    }

    [Fact]
    public async Task GetSellerImageByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
        _sellerImageRepository = new SellerImageRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _sellerImageRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}