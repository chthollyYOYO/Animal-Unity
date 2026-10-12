using System;
using System.Text.Json;

namespace Framework
{
    public static partial class Helper
    {
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string JsonSerialize<T>(T obj)
        {
            string json = JsonSerializer.Serialize(obj, jsonSerializerOptions);
            return json;
        }

        public static T JsonDeserialize<T>(string json, bool error = true)
        {
            try
            {
                T result = JsonSerializer.Deserialize<T>(json, jsonSerializerOptions); ;
                return result;
            }
            catch (Exception ex)
            {
                if (error)
                    LogManager.Error($"反序列化失败 {ex}");
                return default;
            }
        }
    }
}
