namespace RenStore.Catalog.Domain.Constants;

public static class CatalogConstants
{
    public static class Product
    {
        public const int MaxVariantsCount             = 50;
    }

    public static class Image
    {
        public const int MaxImagesCount              = 50;
        
        public const int MaxProductImagePathLength   = 500;
        public const int MinProductImagePathLength   = 15;

        public const long MaxFileSizeBytes           = 50 * 1024 * 1024; /* 50 mb */
        public const long MinFileSizeBytes           = 1;

        public const int MaxImageDimension           = 5000;
        public const int MinImageDimension           = 50;
    
        public const short MaxImageSortOrder         = 50;
        public const short MinImageSortOrder         = 1;
    
        public const short MaxFileNameLaxLength      = 250;
        public const short MinFileNameLaxLength      = 1;
    }

    public static class Price
    {
        public const decimal MinPrice                = 100;
        public const decimal MaxPrice                = 1000000;
    }

    public static class ProductDetail
    {
        public const int MaxDescriptionLength        = 500;
        public const int MinDescriptionLength        = 25;
    
        public const int MaxModelFeaturesLength      = 500;
        public const int MinModelFeaturesLength      = 5;
    
        public const int MaxDecorativeElementsLength = 500;
        public const int MinDecorativeElementsLength = 5;
    
        public const int MaxEquipmentLength          = 500;
        public const int MinEquipmentLength          = 5;
    
        public const int MaxCompositionLength        = 500;
        public const int MinCompositionLength        = 5;
    
        public const int MaxCaringOfThingsLength     = 500;
        public const int MinCaringOfThingsLength     = 5;
    }

    public static class ProductVariant
    {
        public const int MaxProductNameLength        = 500;
        public const int MinProductNameLength        = 10;
    
        public const int MaxUrlLength                = 500;
        public const int MinUrlLength                = 25;

        public const int MaxAttributesCount          = 50;
    }
}