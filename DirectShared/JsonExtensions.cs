using System.Text.Json;

namespace DirectShared;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions Option = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string ToJson<T>(this T obj) =>
        JsonSerializer.Serialize(obj, Option);

    public static T ToObject<T>(this string json) =>
        JsonSerializer.Deserialize<T>(json, Option);
}