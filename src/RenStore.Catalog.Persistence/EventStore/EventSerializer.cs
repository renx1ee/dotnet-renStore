using System.Text.Json;
using System.Text.Json.Serialization;

namespace RenStore.Catalog.Persistence.EventStore;

public static class EventSerializer
{
    public static readonly JsonSerializerOptions Options = new()
    {  
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        
        WriteIndented = false,
        
        PropertyNameCaseInsensitive = true,
        
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        
        IncludeFields = false,
        
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
}