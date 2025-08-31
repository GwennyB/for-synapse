namespace OrderIngest.Utils;

using OrderIngest.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Provides extension methods for (de)serializing objects to/from JSON.
/// </summary>
/// <remarks>This class includes methods that simplify the process of converting objects to their JSON
/// representation with preconfigured serialization options.</remarks>
public static class JsonSerializerExtensions
{
    /// <summary>
    /// Converts an object of type <typeparamref name="T"/> to its JSON representation.
    /// </summary>
    /// <typeparam name="T">The type of the source data to convert.</typeparam>
    /// <param name="content">The content to convert.</param>
    /// <returns>The string representation of the source content.</returns>
    public static string ToJson<T>(this T content)
    {
        JsonSerializerOptions options = new ()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = false
        };
        return JsonSerializer.Serialize(content, options);
    }

    public static Order ConvertJsonToOrder(this string json)
    {
        JsonSerializerOptions options = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        return JsonSerializer.Deserialize<Order>(json);
    }
}
