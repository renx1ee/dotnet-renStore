/*using RenStore.Delivery.Domain.Entities;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums;
using RenStore.Domain.Enums.Clothes;
using Tests.Common;

namespace RenStore.Tests.Common;

public static class TestData
{
    
    
    public static readonly IList<Color> Colors = new[]
    {
        new Color()
        {
            Id = TestDataConstants.ColorIdForUpdate,
            Name = TestDataConstants.ColorNameForUpdate,
            NormalizedName = TestDataConstants.ColorNameForUpdate.ToUpper(),
            NameRu = "колорНейм1",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        },
        new Color()
        {
            Id = TestDataConstants.ColorIdForDelete,
            Name = TestDataConstants.ColorNameForDelete,
            NormalizedName = TestDataConstants.ColorNameForDelete.ToUpper(),
            NameRu = "колорНейм2",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        },
        new Color()
        {
            Id = TestDataConstants.ColorIdForGetting1,
            Name = TestDataConstants.ColorNameForGetting1,
            NormalizedName = TestDataConstants.ColorNameForGetting1.ToUpper(),
            NameRu = "колорНейм3",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        },
        new Color()
        {
            Id = TestDataConstants.ColorIdForGetting2,
            Name = TestDataConstants.ColorNameForGetting2,
            NormalizedName = TestDataConstants.ColorNameForGetting2.ToUpper(),
            NameRu = "колорНейм4",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        },
        new Color()
        {
            Id = TestDataConstants.ColorIdForGetting3,
            Name = TestDataConstants.ColorNameForGetting3,
            NormalizedName = TestDataConstants.ColorNameForGetting3.ToUpper(),
            NameRu = "колорНейм5",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        }
        ,
        new Color()
        {
            Id = TestDataConstants.ColorIdForGetting4,
            Name = TestDataConstants.ColorNameForGetting4,
            NormalizedName = TestDataConstants.ColorNameForGetting4.ToUpper(),
            NameRu = "колорНейм6",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        }
        ,
        new Color()
        {
            Id = TestDataConstants.ColorIdForGetting5,
            Name = TestDataConstants.ColorNameForGetting5,
            NormalizedName = TestDataConstants.ColorNameForGetting5.ToUpper(),
            NameRu = "колорНейм7",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        },
        new Color()
        {
            Id = TestDataConstants.ColorIdForGetting6,
            Name = TestDataConstants.ColorNameForGetting6,
            NormalizedName = TestDataConstants.ColorNameForGetting6.ToUpper(),
            NameRu = "колорНейм8",
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString(),
        }
    };
    
    public static readonly IList<ApplicationUser> Users = new[]
    {   
        // For Create
        new ApplicationUser
        {
            Id = TestDataConstants.UserIdForCreateSeller,
            Name = "1testmail@.com",
            UserName = "1testmail@.com",
            Email = "1testmail@.com",
            PhoneNumber = "0888888881",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        },
        // For Edit
        new ApplicationUser()
        {
            Id = TestDataConstants.UserIdForUpdateSeller,
            Name = "2testmail@.com",
            UserName = "2testmail@.com",
            Email = "2testmail@.com",
            PhoneNumber = "0888888882",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        },
        // For Delete
        new ApplicationUser()
        {
            Id = TestDataConstants.UserIdForDeleteSeller,
            Name = "3testmail@.com",
            UserName = "3testmail@.com",
            Email = "3testmail@.com",
            PhoneNumber = "0888888883",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        },
        new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Name = "testm5323ail@.com",
            UserName = "testm5323ail@.com",
            Email = "testm5323ail@.com",
            PhoneNumber = "5323620243",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        },
        new ApplicationUser()
        {
            Id = TestDataConstants.UserIdForGettingSeller1,
            Name = "7testmail@.com",
            UserName = "7testmail@.com",
            Email = "7testmail@.com",
            PhoneNumber = "0888888888",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        },
        new ApplicationUser()
        {
            Id = TestDataConstants.UserIdForGettingSeller2,
            Name = "4testmail@.com",
            UserName = "4testmail@.com",
            Email = "4testmail@.com",
            PhoneNumber = "0888888884",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        },
        new ApplicationUser()
        {
            Id = TestDataConstants.UserIdForGettingSeller3,
            Name = "5testmail@.com",
            UserName = "5testmail@.com",
            Email = "5testmail@.com",
            PhoneNumber = "0888888885",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        },
        new ApplicationUser()
        {
            Id = TestDataConstants.UserIdForGettingSeller4,
            Name = "6testmail@.com",
            UserName = "6testmail@.com",
            Email = "6testmail@.com",
            PhoneNumber = "0888888886",
            PasswordHash = Guid.NewGuid().ToString(),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        }
    };

    public static readonly IList<SellerEntity> Sellers = new[]
    {
        // For Delete
        new SellerEntity()
        {
            Id = TestDataConstants.SellerIdForDelete,
            Name = TestDataConstants.SellerNameForDelete,
            Description = "Sample Description for Edit",
            NormalizedName = TestDataConstants.SellerNameForDelete.ToUpper(),
            OccuredAt = DateTime.UtcNow,
            ApplicationUserId = TestDataConstants.UserIdForDeleteSeller,
            IsBlocked = false
        },
        // For Edit
        new SellerEntity()
        {
            Id = TestDataConstants.SellerIdForUpdate,
            Name = TestDataConstants.SellerNameForUpdate,
            Description = "Sample Description for Edit",
            NormalizedName = TestDataConstants.SellerNameForUpdate.ToUpper(),
            OccuredAt = DateTime.UtcNow,
            ApplicationUserId = TestDataConstants.UserIdForUpdateSeller,
        },
        new SellerEntity()
        {
            Id = TestDataConstants.SellerIdForGetting1,
            Name = TestDataConstants.SellerNameForGetting1,
            Description = "Sample Description for Edit",
            NormalizedName = TestDataConstants.SellerNameForGetting1.ToUpper(),
            OccuredAt = DateTime.UtcNow,
            ApplicationUserId = TestDataConstants.UserIdForGettingSeller1,
            IsBlocked = false
        },
        new SellerEntity()
        {
            Id = TestDataConstants.SellerIdForGetting2,
            Name = TestDataConstants.SellerNameForGetting2,
            Description = "Sample Description for Edit",
            NormalizedName = TestDataConstants.SellerNameForGetting2.ToUpper(),
            OccuredAt = DateTime.UtcNow,
            ApplicationUserId = TestDataConstants.UserIdForGettingSeller2,
            IsBlocked = true
        },
        new SellerEntity()
        {
            Id = TestDataConstants.SellerIdForGetting3,
            Name = TestDataConstants.SellerNameForGetting3,
            Description = "Sample Description for Edit",
            NormalizedName = TestDataConstants.SellerNameForGetting3.ToUpper(),
            OccuredAt = DateTime.UtcNow,
            ApplicationUserId = TestDataConstants.UserIdForGettingSeller3,
            IsBlocked = false
        },
        new SellerEntity()
        {
            Id = TestDataConstants.SellerIdForGetting4,
            Name = TestDataConstants.SellerNameForGetting4,
            Description = "Sample Description for Edit",
            NormalizedName = TestDataConstants.SellerNameForGetting4.ToUpper(),
            OccuredAt = DateTime.UtcNow.AddHours(1),
            ApplicationUserId = TestDataConstants.UserIdForGettingSeller4,
            IsBlocked = true
        },
    };
    
    /*public static readonly IList<Country> Countries = new[]
    {
        // For delete
        new Country()
        {
            Id = TestDataConstants.CountryIdForDelete,
            Name = TestDataConstants.CountryNameForDelete,
            NormalizedName = TestDataConstants.CountryNameForDelete.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForDelete,
            NormalizedNameRu = TestDataConstants.CountryNameRuForDelete.ToUpper(),
            Code =  "del"
        },
        // For update
        new Country()
        {
            Id = TestDataConstants.CountryIdForUpdate,
            Name = TestDataConstants.CountryNameForUpdate,
            NormalizedName = TestDataConstants.CountryNameForUpdate.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForUpdate,
            NormalizedNameRu = TestDataConstants.CountryNameRuForUpdate.ToUpper(),
            Code =  "upd"
        },
        new Country()
        {
            Id = TestDataConstants.CountryIdForGetting1,
            Name = TestDataConstants.CountryNameForGetting1,
            NormalizedName = TestDataConstants.CountryNameForGetting1.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForGetting1,
            NormalizedNameRu = TestDataConstants.CountryNameRuForGetting1.ToUpper(),
            Code =  "get1"
        },
        new Country()
        {
            Id = TestDataConstants.CountryIdForGetting2,
            Name = TestDataConstants.CountryNameForGetting2,
            NormalizedName = TestDataConstants.CountryNameForGetting2.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForGetting2,
            NormalizedNameRu = TestDataConstants.CountryNameRuForGetting2.ToUpper(),
            Code =  "get2"
        },
        new Country()
        {
            Id = TestDataConstants.CountryIdForGetting3,
            Name = TestDataConstants.CountryNameForGetting3,
            NormalizedName = TestDataConstants.CountryNameForGetting3.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForGetting3,
            NormalizedNameRu = TestDataConstants.CountryNameRuForGetting3.ToUpper(),
            Code =  "get3"
        },
        new Country()
        {
            Id = TestDataConstants.CountryIdForGetting4,
            Name = TestDataConstants.CountryNameForGetting4,
            NormalizedName = TestDataConstants.CountryNameForGetting4.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForGetting4,
            NormalizedNameRu = TestDataConstants.CountryNameRuForGetting4.ToUpper(),
            Code =  "get4",
            OtherName = TestDataConstants.CountryOtherNameForGetting4,
            NormalizedOtherName = TestDataConstants.CountryOtherNameForGetting4.ToUpper(),
        },
        new Country()
        {
            Id = TestDataConstants.CountryIdForGetting5,
            Name = TestDataConstants.CountryNameForGetting5,
            NormalizedName = TestDataConstants.CountryNameForGetting5.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForGetting5,
            NormalizedNameRu = TestDataConstants.CountryNameRuForGetting5.ToUpper(),
            Code =  "get5",
        },
        new Country()
        {
            Id = TestDataConstants.CountryIdForGetting6,
            Name = TestDataConstants.CountryNameForGetting6,
            NormalizedName = TestDataConstants.CountryNameForGetting6.ToUpper(),
            NameRu = TestDataConstants.CountryNameRuForGetting6,
            NormalizedNameRu = TestDataConstants.CountryNameRuForGetting6.ToUpper(),
            Code =  "get6",
            OtherName = TestDataConstants.CountryOtherNameForGetting6,
            NormalizedOtherName = TestDataConstants.CountryOtherNameForGetting6.ToUpper(),
        }
    };#1#
    
    /*public static readonly IList<City> Cities = new[]
    {
        // For Edit
        new City()
        {
            Id = TestDataConstants.CityIdForUpdate,
            Name = TestDataConstants.CityNameForUpdate,
            NormalizedName = TestDataConstants.CityNameForUpdate.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForUpdate,
            NormalizedNameRu = TestDataConstants.CityNameRuForUpdate.ToUpper(),
            CountryId = TestDataConstants.CountryIdForUpdate
        },
        // For Edit
        new City()
        {
            Id = TestDataConstants.CityIdForDelete,
            Name = TestDataConstants.CityNameForDelete,
            NormalizedName = TestDataConstants.CityNameForDelete.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForDelete,
            NormalizedNameRu = TestDataConstants.CityNameRuForDelete.ToUpper(),
            CountryId = TestDataConstants.CountryIdForDelete
        },
        new City()
        {
            Id = TestDataConstants.CityIdForGetting1,
            Name = TestDataConstants.CityNameForGetting1,
            NormalizedName = TestDataConstants.CityNameForGetting1.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForGetting1,
            NormalizedNameRu = TestDataConstants.CityNameRuForGetting1.ToUpper(),
            CountryId = TestDataConstants.CountryIdForGetting1
        },
        new City()
        {
            Id = TestDataConstants.CityIdForGetting2,
            Name = TestDataConstants.CityNameForGetting2,
            NormalizedName = TestDataConstants.CityNameForGetting2.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForGetting2,
            NormalizedNameRu = TestDataConstants.CityNameRuForGetting2.ToUpper(),
            CountryId = TestDataConstants.CountryIdForGetting2
        },
        new City()
        {
            Id = TestDataConstants.CityIdForGetting3,
            Name = TestDataConstants.CityNameForGetting3,
            NormalizedName = TestDataConstants.CityNameForGetting3.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForGetting3,
            NormalizedNameRu = TestDataConstants.CityNameRuForGetting3.ToUpper(),
            CountryId = TestDataConstants.CountryIdForGetting3
        },
        new City()
        {
            Id = TestDataConstants.CityIdForGetting4,
            Name = TestDataConstants.CityNameForGetting4,
            NormalizedName = TestDataConstants.CityNameForGetting4.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForGetting4,
            NormalizedNameRu = TestDataConstants.CityNameRuForGetting4.ToUpper(),
            CountryId = TestDataConstants.CountryIdForGetting4
        },
        new City()
        {
            Id = TestDataConstants.CityIdForGetting5,
            Name = TestDataConstants.CityNameForGetting5,
            NormalizedName = TestDataConstants.CityNameForGetting5.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForGetting5,
            NormalizedNameRu = TestDataConstants.CityNameRuForGetting5.ToUpper(),
            CountryId = TestDataConstants.CountryIdForGetting5
        },
        new City()
        {
            Id = TestDataConstants.CityIdForGetting6,
            Name = TestDataConstants.CityNameForGetting6,
            NormalizedName = TestDataConstants.CityNameForGetting6.ToUpper(),
            NameRu = TestDataConstants.CityNameRuForGetting6,
            NormalizedNameRu = TestDataConstants.CityNameRuForGetting6.ToUpper(),
            CountryId = TestDataConstants.CountryIdForGetting6
        }
    };#1#
    
    public static readonly IList<Category> Categories = new[]
    {
        new Category()
        {
            Id = TestDataConstants.CategoryIdForUpdate,
            Name = TestDataConstants.CategoryNameForUpdate,
            NormalizedName = TestDataConstants.CategoryNameForUpdate.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForUpdate,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForUpdate.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForDelete,
            Name = TestDataConstants.CategoryNameForDelete,
            NormalizedName = TestDataConstants.CategoryNameForDelete.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForDelete,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForDelete.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForGetting1,
            Name = TestDataConstants.CategoryNameForGetting1,
            NormalizedName = TestDataConstants.CategoryNameForGetting1.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting1,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting1.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForGetting2,
            Name = TestDataConstants.CategoryNameForGetting2,
            NormalizedName = TestDataConstants.CategoryNameForGetting2.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting2,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting2.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForGetting3,
            Name = TestDataConstants.CategoryNameForGetting3,
            NormalizedName = TestDataConstants.CategoryNameForGetting3.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting3,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting3.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForGetting4,
            Name = TestDataConstants.CategoryNameForGetting4,
            NormalizedName = TestDataConstants.CategoryNameForGetting4.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting4,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting4.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForGetting5,
            Name = TestDataConstants.CategoryNameForGetting5,
            NormalizedName = TestDataConstants.CategoryNameForGetting5.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting5,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting5.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForGetting6,
            Name = TestDataConstants.CategoryNameForGetting6,
            NormalizedName = TestDataConstants.CategoryNameForGetting6.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting6,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting6.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Category()
        {
            Id = TestDataConstants.CategoryIdForGetting7,
            Name = TestDataConstants.CategoryNameForGetting7,
            NormalizedName = TestDataConstants.CategoryNameForGetting7.ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting7,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting7.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
    };
    
    public static readonly IList<SubCategory> SubCategories = new[]
    {
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForUpdate,
            Name = TestDataConstants.SubCategoryNameForUpdate,
            NormalizedName = TestDataConstants.SubCategoryNameForUpdate.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForUpdate,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForUpdate.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForUpdate,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForDelete,
            Name = TestDataConstants.SubCategoryNameForDelete,
            NormalizedName = TestDataConstants.SubCategoryNameForDelete.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForDelete,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForDelete.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForDelete,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForGetting1,
            Name = TestDataConstants.SubCategoryNameForGetting1,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting1.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting1,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting1.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting1,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForGetting2,
            Name = TestDataConstants.SubCategoryNameForGetting2,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting2.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting2,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting2.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting2,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForGetting3,
            Name = TestDataConstants.SubCategoryNameForGetting3,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting3.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting3,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting3.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting3,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForGetting4,
            Name = TestDataConstants.SubCategoryNameForGetting4,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting4.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting4,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting4.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting4,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForGetting5,
            Name = TestDataConstants.SubCategoryNameForGetting5,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting5.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting5,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting5.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting5,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForGetting6,
            Name = TestDataConstants.SubCategoryNameForGetting6,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting6.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting6,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting6.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting6,
        },
        new SubCategory()
        {
            Id = TestDataConstants.SubCategoryIdForGetting7,
            Name = TestDataConstants.SubCategoryNameForGetting7,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting7.ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting7,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting7.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting7,
        },
    };

    public static readonly IList<Product> Products = new[]
    {
        // For Delete
        new Product()
        {
            Id = TestDataConstants.ProductIdForDelete,
            IsBlocked = false,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        // For Edit
        new Product()
        {
            Id = TestDataConstants.ProductIdForUpdate,
            IsBlocked = false,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        new Product()
        {
            Id = TestDataConstants.ProductIdForGetting1,
            IsBlocked = false,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        new Product()
        {
            Id = TestDataConstants.ProductIdForGetting2,
            IsBlocked = false,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        new Product()
        {
            Id = TestDataConstants.ProductIdForGetting3,
            IsBlocked = true,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        new Product()
        {
            Id = TestDataConstants.ProductIdForGetting4,
            IsBlocked = false,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        new Product()
        {
            Id = TestDataConstants.ProductIdForGetting5,
            IsBlocked = true,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        new Product()
        {
            Id = TestDataConstants.ProductIdForGetting6,
            IsBlocked = false,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        },
        new Product()
        {
            Id = TestDataConstants.ProductIdForGetting7,
            IsBlocked = false,
            OverallRating = 0,
            SellerId = TestDataConstants.SellerIdForGetting1,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        }
    };

    public static readonly IList<ProductVariant> ProductVariants = new[]
    {
        // For Edit
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForUpdate,
            Name = TestDataConstants.ProductVariantNameForUpdate,
            NormalizedName = TestDataConstants.ProductVariantNameForUpdate.ToUpper(),
            Rating = 0,
            Article = TestDataConstants.ProductVariantArticleForUpdate,
            InStock = 36,
            IsAvailable = false,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForUpdate,
            ColorId = TestDataConstants.ColorIdForUpdate
        },
        // For Delete
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForDelete,
            Name = TestDataConstants.ProductVariantNameForDelete,
            NormalizedName = TestDataConstants.ProductVariantNameForDelete.ToUpper(),
            Rating = 3,
            Article = TestDataConstants.ProductVariantArticleForDelete,
            InStock = 86,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForDelete,
            ColorId = TestDataConstants.ColorIdForDelete
        },
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForGetting1,
            Name = TestDataConstants.ProductVariantNameForGetting1,
            NormalizedName = TestDataConstants.ProductVariantNameForGetting1.ToUpper(),
            Rating = 4,
            Article = TestDataConstants.ProductVariantArticleForGetting1,
            InStock = 235,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForGetting1,
            ColorId = TestDataConstants.ColorIdForGetting1
        },
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForGetting2,
            Name = TestDataConstants.ProductVariantNameForGetting2,
            NormalizedName = TestDataConstants.ProductVariantNameForGetting2.ToUpper(),
            Rating = 1,
            Article = TestDataConstants.ProductVariantArticleForGetting2,
            InStock = 96,
            IsAvailable = false,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForGetting2,
            ColorId = TestDataConstants.ColorIdForGetting2
        },
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForGetting3,
            Name = TestDataConstants.ProductVariantNameForGetting3,
            NormalizedName = TestDataConstants.ProductVariantNameForGetting3.ToUpper(),
            Rating = 5,
            Article = TestDataConstants.ProductVariantArticleForGetting3,
            InStock = 62,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForGetting3,
            ColorId = TestDataConstants.ColorIdForGetting3
        },
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForGetting4,
            Name = TestDataConstants.ProductVariantNameForGetting4,
            NormalizedName = TestDataConstants.ProductVariantNameForGetting4.ToUpper(),
            Rating = 4,
            Article = TestDataConstants.ProductVariantArticleForGetting4,
            InStock = 736,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForGetting4,
            ColorId = TestDataConstants.ColorIdForGetting4
        },
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForGetting5,
            Name = TestDataConstants.ProductVariantNameForGetting5,
            NormalizedName = TestDataConstants.ProductVariantNameForGetting5.ToUpper(),
            Rating = 3,
            Article = TestDataConstants.ProductVariantArticleForGetting5,
            InStock = 332,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForGetting5,
            ColorId = TestDataConstants.ColorIdForGetting5
        },
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForGetting6,
            Name = TestDataConstants.ProductVariantNameForGetting6,
            NormalizedName = TestDataConstants.ProductVariantNameForGetting6.ToUpper(),
            Rating = 3,
            Article = TestDataConstants.ProductVariantArticleForGetting6,
            InStock = 606,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForGetting6,
            ColorId = TestDataConstants.ColorIdForGetting6
        },
        new ProductVariant()
        {
            Id = TestDataConstants.ProductVariantIdForGetting7,
            Name = TestDataConstants.ProductVariantNameForGetting7,
            NormalizedName = TestDataConstants.ProductVariantNameForGetting7.ToUpper(),
            Rating = 3,
            Article = TestDataConstants.ProductVariantArticleForGetting7,
            InStock = 32,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            Url = "",
            ProductId = TestDataConstants.ProductIdForGetting7,
            ColorId = TestDataConstants.ColorIdForGetting6
        }
    };

    public static readonly IList<ProductClothEntity> ProductClothes = new[]
    {
        // For Edit
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForUpdate,
            Gender = Gender.Unisex,
            Season = Season.Autumn,
            Neckline = Neckline.BoatNeck,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForUpdate
        },
        // For Delete
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForDelete,
            Gender = Gender.Unisex,
            Season = Season.Summer,
            Neckline = Neckline.ScoopNeck,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForDelete
        },
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForGetting1,
            Gender = Gender.Woman,
            Season = Season.Spring,
            Neckline = Neckline.ScoopNeck,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForGetting1
        },
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForGetting2,
            Gender = Gender.Man,
            Season = Season.Winter,
            Neckline = Neckline.Horseshoe,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForGetting2
        },
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForGetting3,
            Gender = Gender.Unisex,
            Season = Season.YearRound,
            Neckline = Neckline.PoloNeck,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForGetting3
        },
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForGetting4,
            Gender = Gender.Man,
            Season = Season.Summer,
            Neckline = Neckline.Round,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForGetting4
        },
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForGetting5,
            Gender = Gender.Unisex,
            Season = Season.YearRound,
            Neckline = Neckline.Square,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForGetting5
        },
        new ProductClothEntity()
        {
            Id = TestDataConstants.ProductClothIdForGetting6,
            Gender = Gender.Woman,
            Season = Season.Autumn,
            Neckline = Neckline.Round,
            TheCut = TheCut.Free,
            ProductId = TestDataConstants.ProductIdForGetting6
        },
    };

    public static readonly IList<ProductAttributeEntity> ProductAttributes = new[]
    {
        // For Edit
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductAttributeIdForUpdate,
            Name = "attribute_name_for_update",
            Value = "attribute_value_for_update",
            ProductVariantId = TestDataConstants.ProductVariantIdForUpdate
        },
        // For Delete
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductAttributeIdForDelete,
            Name = "attribute_name_for_delete",
            Value = "attribute_value_for_delete",
            ProductVariantId = TestDataConstants.ProductVariantIdForDelete
        },
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductAttributeIdForGetting1,
            Name = "attribute_name_for_getting_1",
            Value = "attribute_value_for_getting_1",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
        },
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductVariantIdForGetting2,
            Name = "attribute_name_for_getting_2",
            Value = "attribute_value_for_getting_2",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting2
        },
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductVariantIdForGetting3,
            Name = "attribute_name_for_getting_3",
            Value = "attribute_value_for_getting_3",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting3
        },
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductVariantIdForGetting4,
            Name = "attribute_name_for_getting_4",
            Value = "attribute_value_for_getting_4",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting4
        },
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductVariantIdForGetting5,
            Name = "attribute_name_for_getting_5",
            Value = "attribute_value_for_getting_5",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting5
        },
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductVariantIdForGetting6,
            Name = "attribute_name_for_getting_6",
            Value = "attribute_value_for_getting_6",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting6
        },
        new ProductAttributeEntity()
        {
            Id = TestDataConstants.ProductVariantIdForGetting7,
            Name = "attribute_name_for_getting_7",
            Value = "attribute_value_for_getting_7",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting7
        },
        
    };

    public static readonly IList<ProductDetailEntity> ProductDetails = new[]
    {
        // For Edit
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForUpdate,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Box,
            CountryOfManufactureId = TestDataConstants.CountryIdForUpdate,
            ProductVariantId = TestDataConstants.ProductVariantIdForUpdate
        },
        // For Delete
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForDelete,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Box,
            CountryOfManufactureId = TestDataConstants.CountryIdForDelete,
            ProductVariantId = TestDataConstants.ProductVariantIdForDelete
        },
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForGetting1,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Box,
            CountryOfManufactureId = TestDataConstants.CountryIdForGetting1,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
        },
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForGetting2,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Package,
            CountryOfManufactureId = TestDataConstants.CountryIdForGetting2,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting2
        },
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForGetting3,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Package,
            CountryOfManufactureId = TestDataConstants.CountryIdForGetting3,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting3
        },
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForGetting4,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Box,
            CountryOfManufactureId = TestDataConstants.CountryIdForGetting4,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting4
        },
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForGetting5,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Package,
            CountryOfManufactureId = TestDataConstants.CountryIdForGetting5,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting5
        },
        new ProductDetailEntity()
        {
            Id = TestDataConstants.ProductDetailIdForGetting6,
            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
            Composition = "Composition",
            CaringOfThings = "CaringOfThings",
            TypeOfPacking = TypeOfPackaging.Box,
            CountryOfManufactureId = TestDataConstants.CountryIdForGetting6,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting6
        }
    };

    public static readonly IList<ProductClothSizeEntity> ProductClothSizes = new[]
    {
        // For Edit
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForUpdate,
            ClothSize = ClothesSizes.S,
            Amount = 34,
            ProductClothId = TestDataConstants.ProductClothIdForUpdate
        },
        // For Delete
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForDelete,
            ClothSize = ClothesSizes.L,
            Amount = 18,
            ProductClothId = TestDataConstants.ProductClothIdForDelete
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting1,
            ClothSize = ClothesSizes.M,
            Amount = 19,
            ProductClothId = TestDataConstants.ProductClothIdForGetting1
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting2,
            ClothSize = ClothesSizes.XS,
            Amount = 24,
            ProductClothId = TestDataConstants.ProductClothIdForGetting2
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting3,
            ClothSize = ClothesSizes.S,
            Amount = 15,
            ProductClothId = TestDataConstants.ProductClothIdForGetting3
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting4,
            ClothSize = ClothesSizes.XXS,
            Amount = 64,
            ProductClothId = TestDataConstants.ProductClothIdForGetting4
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting5,
            ClothSize = ClothesSizes.S,
            Amount = 325,
            ProductClothId = TestDataConstants.ProductClothIdForGetting5
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting6,
            ClothSize = ClothesSizes.XL,
            Amount = 90,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting7,
            ClothSize = ClothesSizes.XXS,
            Amount = 3,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting8,
            ClothSize = ClothesSizes.XS,
            Amount = 62,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting9,
            ClothSize = ClothesSizes.S,
            Amount = 84,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting10,
            ClothSize = ClothesSizes.M,
            Amount = 14,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting11,
            ClothSize = ClothesSizes.L,
            Amount = 53,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting12,
            ClothSize = ClothesSizes.XL,
            Amount = 8,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting13,
            ClothSize = ClothesSizes.XXL,
            Amount = 2,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting14,
            ClothSize = ClothesSizes.XXL,
            Amount = 85,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        },
        new ProductClothSizeEntity()
        {
            Id = TestDataConstants.ProductClothSizeIdForGetting15,
            ClothSize = ClothesSizes.XXXL,
            Amount = 44,
            ProductClothId = TestDataConstants.ProductClothIdForGetting6
        }
    };

    public static readonly IList<ProductPriceHistoryEntity> PriceHistories = new[]
    {
        // For Edit
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForUpdate,
            Price = 2990,
            OldPrice = 3990,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForUpdate
        },
        // For Delete
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForDelete,
            Price = 3000,
            OldPrice = 5990,
            DiscountPrice = 2990,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForDelete
        },
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForGetting1,
            Price = 2990,
            OldPrice = 3990,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
        },
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForGetting2,
            Price = 2990,
            OldPrice = 3990,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting2
        },
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForGetting3,
            Price = 2990,
            OldPrice = 3990,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting3
        },
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForGetting4,
            Price = 2990,
            OldPrice = 3990,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting4
        },
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForGetting5,
            Price = 2990,
            OldPrice = 3990,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting5
        },
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForGetting6,
            Price = 2990,
            OldPrice = 3990,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting6
        },
        new ProductPriceHistoryEntity()
        {
            Id = TestDataConstants.ProductPriceHistoryIdForGetting7,
            Price = 4000,
            OldPrice = 5000,
            DiscountPrice = 1000,
            DiscountPercent = 0,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            ChangedBy = "seller",
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting6
        },
    };

    public static readonly IList<ProductImageEntity> ProductImages = new[]
    {
        // For Edit
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForUpdate,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForUpdate
        },
        // For Delete
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForDelete,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForDelete
        },
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForGetting1,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
        },
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForGetting2,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting2
        },
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForGetting3,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting3
        },
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForGetting4,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting4
        },
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForGetting5,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting5
        },
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForGetting6,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting6
        },
        new ProductImageEntity()
        {
            Id = TestDataConstants.ProductImageIdForGetting7,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting7
        },
    };
    
    public static readonly IList<SellerImageEntity> SellerImages = new[]
    {
        // For Edit
        new SellerImageEntity()
        {
            Id = TestDataConstants.SellerImageIdForUpdate,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            SellerId = TestDataConstants.SellerIdForUpdate
        },
        // For Delete
        new SellerImageEntity()
        {
            Id = TestDataConstants.SellerImageIdForDelete,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            SellerId = TestDataConstants.SellerIdForDelete
        },
        new SellerImageEntity()
        {
            Id = TestDataConstants.SellerImageIdForGetting1,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            SellerId = TestDataConstants.SellerIdForGetting1
        },
        new SellerImageEntity()
        {
            Id = TestDataConstants.SellerImageIdForGetting2,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            SellerId = TestDataConstants.SellerIdForGetting2
        },
        new SellerImageEntity()
        {
            Id = TestDataConstants.SellerImageIdForGetting3,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            SellerId = TestDataConstants.SellerIdForGetting3
        },
        new SellerImageEntity()
        {
            Id = TestDataConstants.SellerImageIdForGetting4,
            OriginalFileName = Guid.NewGuid().ToString(),
            StoragePath = Guid.NewGuid().ToString() + "/" + Guid.NewGuid().ToString(),
            FileSizeBytes = 500,
            IsMain = true,
            SortOrder = 1,
            UploadedAt = DateTime.UtcNow,
            Weight = 500,
            Height = 500,
            SellerId = TestDataConstants.SellerIdForGetting4
        }
    };

    public static readonly IList<ShoppingCartEntity> ShoppingCarts = new[]
    {
        new ShoppingCartEntity()
        {
            Id = TestDataConstants.ShoppingCartIdForUpdate,
            TotalPrice = 23542,
            OccuredAt = DateTime.UtcNow,
            UserId = TestDataConstants.UserIdForUpdateSeller
        },
        new ShoppingCartEntity()
        {
            Id = TestDataConstants.ShoppingCartIdForDelete,
            TotalPrice = 2534,
            OccuredAt = DateTime.UtcNow,
            UserId = TestDataConstants.UserIdForDeleteSeller
        },
        new ShoppingCartEntity()
        {
            Id = TestDataConstants.ShoppingCartIdForGetting1,
            TotalPrice = 854,
            OccuredAt = DateTime.UtcNow,
            UserId = TestDataConstants.UserIdForGettingSeller1
        },
        new ShoppingCartEntity()
        {
            Id = TestDataConstants.ShoppingCartIdForGetting2,
            TotalPrice = 21456,
            OccuredAt = DateTime.UtcNow,
            UserId = TestDataConstants.UserIdForGettingSeller2
        },
        new ShoppingCartEntity()
        {
            Id = TestDataConstants.ShoppingCartIdForGetting3,
            TotalPrice = 8795,
            OccuredAt = DateTime.UtcNow,
            UserId = TestDataConstants.UserIdForGettingSeller3
        },
    };

    public static readonly IList<ShoppingCartItemEntity> ShoppingCartItems = new[]
    {
        new ShoppingCartItemEntity()
        {
            Id = TestDataConstants.ShoppingCartItemIdForUpdate,
            Quantity = 1,
            Price = 3632,
            CartId = TestDataConstants.ShoppingCartIdForUpdate,
            ProductId = TestDataConstants.ProductIdForUpdate
        },
        new ShoppingCartItemEntity()
        {
            Id = TestDataConstants.ShoppingCartItemIdForDelete,
            Quantity = 1,
            Price = 32526,
            CartId = TestDataConstants.ShoppingCartIdForDelete,
            ProductId = TestDataConstants.ProductIdForDelete
        },
        new ShoppingCartItemEntity()
        {
            Id = TestDataConstants.ShoppingCartItemIdForGetting1,
            Quantity = 1,
            Price = 7474,
            CartId = TestDataConstants.ShoppingCartIdForGetting1,
            ProductId = TestDataConstants.ProductIdForGetting1
        },
        new ShoppingCartItemEntity()
        {
            Id = TestDataConstants.ShoppingCartItemIdForGetting2,
            Quantity = 1,
            Price = 235,
            CartId = TestDataConstants.ShoppingCartIdForGetting2,
            ProductId = TestDataConstants.ProductIdForGetting2
        },
        new ShoppingCartItemEntity()
        {
            Id = TestDataConstants.ShoppingCartItemIdForGetting3,
            Quantity = 1,
            Price = 747,
            CartId = TestDataConstants.ShoppingCartIdForGetting3,
            ProductId = TestDataConstants.ProductIdForGetting3
        },
        new ShoppingCartItemEntity()
        {
            Id = TestDataConstants.ShoppingCartItemIdForGetting4,
            Quantity = 1,
            Price = 353253,
            CartId = TestDataConstants.ShoppingCartIdForGetting3,
            ProductId = TestDataConstants.ProductIdForGetting4
        },
        new ShoppingCartItemEntity()
        {
            Id = TestDataConstants.ShoppingCartItemIdForGetting5,
            Quantity = 1,
            Price = 325326,
            CartId = TestDataConstants.ShoppingCartIdForGetting3,
            ProductId = TestDataConstants.ProductIdForGetting5
        },
    };
    // Todo:
    public static readonly IList<AnswerComplainEntity> AnswerComplains = new[]
    {
        new AnswerComplainEntity()
        {
            Id = TestDataConstants.AnswerComplainIdForUpdate,
            CustomReason = Guid.NewGuid().ToString(),
            Comment = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
        }
    };

    public static readonly IList<ProductVariantComplainEntity> ProductVariantComplains = new[]
    {
        new ProductVariantComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForUpdate,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForUpdate,
            UserId = TestDataConstants.UserIdForUpdateSeller
        },
        new ProductVariantComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForDelete,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForDelete,
            UserId = TestDataConstants.UserIdForDeleteSeller
        },
        new ProductVariantComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForGetting1,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting1,
            UserId = TestDataConstants.UserIdForGettingSeller1
        },
        new ProductVariantComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForGetting2,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting2,
            UserId = TestDataConstants.UserIdForGettingSeller2
        },
        new ProductVariantComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForGetting3,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = ProductComplainStatus.New,
            ProductVariantId = TestDataConstants.ProductVariantIdForGetting3,
            UserId = TestDataConstants.UserIdForGettingSeller3
        },
    };

    public static readonly IList<SellerComplainEntity> SellerComplains = new[]
    {
        new SellerComplainEntity
        {
            Id = TestDataConstants.SellerComplainIdForUpdate,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId = TestDataConstants.SellerIdForUpdate,
            UserId = TestDataConstants.UserIdForUpdateSeller
        },
        new SellerComplainEntity
        {
            Id = TestDataConstants.SellerComplainIdForDelete,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId = TestDataConstants.SellerIdForDelete,
            UserId = TestDataConstants.UserIdForDeleteSeller
        },
        new SellerComplainEntity
        {
            Id = TestDataConstants.SellerComplainIdForGetting1,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId = TestDataConstants.SellerIdForGetting1,
            UserId = TestDataConstants.UserIdForGettingSeller1
        },
        new SellerComplainEntity
        {
            Id = TestDataConstants.SellerComplainIdForGetting2,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId= TestDataConstants.SellerIdForGetting2,
            UserId = TestDataConstants.UserIdForGettingSeller2
        },
        new SellerComplainEntity
        {
            Id = TestDataConstants.SellerComplainIdForGetting3,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedAt = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId = TestDataConstants.SellerIdForGetting3,
            UserId = TestDataConstants.UserIdForGettingSeller3
        },
    };
    
    /*public static readonly IList<ReviewComplainEntity> ReviewComplains = new[]
    {
        new ReviewComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForUpdate,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            OccuredAt = DateTime.UtcNow,
            Status = ReviewComplainStatus.New,
            ReviewId = TestDataConstants.SellerIdForUpdate,
            UserId = TestDataConstants.UserIdForUpdateSeller
        },
        new ReviewComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForDelete,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            OccuredAt = DateTime.UtcNow,
            Status = ReviewComplainStatus.New,
            ReviewId = TestDataConstants.SellerIdForDelete,
            UserId = TestDataConstants.UserIdForDeleteSeller
        },
        new ReviewComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForGetting1,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            OccuredAt = DateTime.UtcNow,
            Status = ReviewComplainStatus.New,
            ReviewId = TestDataConstants.SellerIdForGetting1,
            UserId = TestDataConstants.UserIdForGettingSeller1
        },
        new ReviewComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForGetting2,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            OccuredAt = DateTime.UtcNow,
            Status = ReviewComplainStatus.New,
            ReviewId= TestDataConstants.SellerIdForGetting2,
            UserId = TestDataConstants.UserIdForGettingSeller2
        },
        new ReviewComplainEntity
        {
            Id = TestDataConstants.ProductVariantComplainIdForGetting3,
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            OccuredAt = DateTime.UtcNow,
            Status = ReviewComplainStatus.New,
            ReviewId = TestDataConstants.SellerIdForGetting3,
            UserId = TestDataConstants.UserIdForGettingSeller3
        },
    };#1#
}*/