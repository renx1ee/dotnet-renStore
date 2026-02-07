using RenStore.Catalog.Domain.Constants;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.ValueObjects;

public sealed class Size
{
    public LetterSize LetterSize { get; }
    public decimal? Number { get; set; }
    public SizeType Type { get; }
    public SizeSystem System { get; }

    private Size(
        LetterSize letterSize,
        decimal? number,
        SizeType type,
        SizeSystem system)
    {
        LetterSize = letterSize;
        Type = type;
        System = system;
        Number = number;
    }

    public static Size Create(
        LetterSize size,
        SizeType type,
        SizeSystem system)
    {
        decimal number = (system, type) switch
        {
            (SizeSystem.RU, SizeType.Clothes) => SizeDictionaries.ClothSizesRu[size],
            (SizeSystem.RU, SizeType.Shoes)   => SizeDictionaries.ClothSizesRu[size],
            (SizeSystem.EU, SizeType.Clothes) => SizeDictionaries.ClothSizesEu[size],
            (SizeSystem.EU, SizeType.Shoes)   => SizeDictionaries.ClothSizesEu[size],
            (SizeSystem.US, SizeType.Clothes) => SizeDictionaries.ClothSizesUs[size],
            (SizeSystem.US, SizeType.Shoes)   => SizeDictionaries.ClothSizesUs[size],
            _ => throw new DomainException("Unsupported letterSize sizeSystem")
        };

        return new Size( 
            letterSize: size, 
            type: type, 
            number: number, 
            system: system); 
    }

    public static void Validate(
        LetterSize size,
        SizeType type,
        SizeSystem system)
    {
        
    }
}