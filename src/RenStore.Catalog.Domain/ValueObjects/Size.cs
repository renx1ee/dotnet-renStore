using RenStore.Catalog.Domain.Constants;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.ValueObjects;

/// <summary>
/// Represents a product size with international system conversions.
/// Provides unified size handling across different measurement systems and product types.
/// </summary>
public sealed class Size
{
    /// <summary>
    /// Alphanumeric size designation.
    /// </summary>
    public LetterSize LetterSize { get; }
    
    /// <summary>
    /// Numeric equivalent of the letter size in the specified measurement system.
    /// </summary>
    public decimal? Number { get; }
    
    /// <summary>
    /// Category of product sizing (Clothes or Shoes).
    /// </summary>
    public SizeType Type { get; }
    
    /// <summary>
    /// Measurement system.
    /// </summary>
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

    /// <summary>
    /// Creates a Size value object with automatic numeric conversion.
    /// </summary>
    /// <param name="size">Alphanumeric size designation</param>
    /// <param name="type">Product type (Clothes/Shoes)</param>
    /// <param name="system">Measurement system (RU/US/EU)</param>
    /// <returns>A new Size value object with converted numeric value</returns>
    /// <exception cref="DomainException">
    /// Thrown when the combination of size, type, and system is unsupported
    /// </exception>
    /// <remarks>
    /// Automatically converts letter sizes to their numeric equivalents using predefined dictionaries.
    /// The conversion depends on both the measurement system and product type.
    /// </remarks>
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
    
    // TODO:
    public static void Validate(
        LetterSize size,
        SizeType type,
        SizeSystem system)
    {
        // TODO:
    }
}