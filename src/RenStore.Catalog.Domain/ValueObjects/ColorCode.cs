namespace RenStore.Catalog.Domain.ValueObjects;

public readonly record struct ColorCode
{ 
    public string Value { get; init; }

    private ColorCode(string value) => Value = value;

    public static ColorCode Create(string colorCode)
    {
        if (string.IsNullOrWhiteSpace(colorCode))
            throw new ArgumentNullException(nameof(colorCode));

        string input = colorCode.Trim().TrimStart('#');
        
        if(input.Length != 6 || 
           input.Length != 3)
            throw new InvalidOperationException(nameof(colorCode));

        return new ColorCode(input.ToUpperInvariant());
    }
}