/*using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RenStore.Application.Abstractions.Repository;
using RenStore.Delivery.Domain.Entities;
using RenStore.Domain.Entities;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using Tests.Common;

namespace RenStore.Tests.Repository.AddressRepositoryTests;

public class AddTests
{
    private IAddressQuery _addressRepository;
    
    [Fact]
    public async Task CreateAddressAsync_Success_Test()
    {
        // Arrange
        await using var context = TestDatabaseFixture.CreateReadyContext();

        var repository = CreateRepository(context);
        
        var address = new Address()
        {
            HouseCode = "HC-001",
            Street = "Main street",
            BuildingNumber = "10",
            ApartmentNumber = "5",
            FullAddressEn = "Main street 10-5",
            ApplicationUserId = TestDataConstants.UserIdForGettingSeller1,
            CountryId = 552,
            CityId = 52
        };

        // Act
        var id = await repository.AddAsync(address, CancellationToken.None);

        // Assert
        
        var assertContext =  TestDatabaseFixture.CreateReadyContext();

        var result = await assertContext.Addresses
            .FirstOrDefaultAsync(x => x.Id == id);

        Assert.NotNull(result);
        Assert.Equal(result.HouseCode, address.HouseCode);
        Assert.Equal(result.Street, address.Street);
        Assert.Equal(result.BuildingNumber, address.BuildingNumber);
        Assert.Equal(result.ApartmentNumber, address.ApartmentNumber);
        Assert.Equal(result.FullAddressEn, address.FullAddressEn);
        Assert.Equal(result.ApplicationUserId, address.ApplicationUserId);
        Assert.Equal(result.CountryId, address.CountryId);
        Assert.Equal(result.CityId, address.CityId);
    }

    private static IAddressQuery CreateRepository(ApplicationDbContext context)
    {
        var logger = LoggerFactory
            .Create(builder => builder.AddConsole())
            .CreateLogger<AddressRepository>();
        
        IAddressQuery repository = new AddressRepository(logger, context);

        return repository;
    }
}*/