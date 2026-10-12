using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework
{
    public class SubclassJsonConverter<T> : JsonConverter<T>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T).IsAssignableFrom(typeToConvert);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty(".type", out var typeElement))
            {
                string type = typeElement.GetString();
                T obj = (T)JsonSerializer.Deserialize(root.GetRawText(), Type.GetType(type), options);
                return obj;
            }
            LogManager.Error("Json Read Error");
            return default;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(".type", value.GetType().FullName);

            var valueType = value.GetType();
            var fields = valueType.GetFields();
            foreach (var field in fields)
            {
                writer.WritePropertyName(field.Name);
                JsonSerializer.Serialize(writer, field.GetValue(value), field.FieldType, options);
            }

            var properties = valueType.GetProperties();
            foreach (var property in properties)
            {
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, property.GetValue(value), property.PropertyType, options);
            }

            writer.WriteEndObject();
        }
    }
}