using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestMobiBuy.Share.Converters;

[ExcludeFromCodeCoverage]
public class JsonIsoDateTimeOffSetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString() ?? string.Empty;
        return string.IsNullOrWhiteSpace(dateString) ? DateTimeOffset.MinValue : DateTimeOffset.Parse(dateString);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        string format = "yyyy-MM-ddTHH:mm:ss.fffffff";
        writer.WriteStringValue(value.ToString(format));
    }
}
