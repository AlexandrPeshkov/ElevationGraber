using Newtonsoft.Json;
using System;

namespace ElevationGraber.Extensions
{
    class JsonConverterExtensions
    {
    }

    public class DoubleFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(double);

        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue($"{value:0.00}");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(decimal))
            {
                Decimal.TryParse((reader.Value.ToString().Replace(",", ".")), out var value);
                return value;
            }
            return default;
        }
    }
}
