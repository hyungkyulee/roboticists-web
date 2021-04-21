using System;
using System.Text.Json;

namespace RoboticistsApis.Infrastructure
{
    public static class JsonExtensions
    {
        public static (T entity, Exception error) Deserialize<T>(this string payload)
        {
            if (string.IsNullOrEmpty(payload))
            {
                return (default, new Exception("A payload cannot be empty"));
            }

            var entity = JsonSerializer.Deserialize<T>(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            Console.WriteLine(entity.ToJson());

            return (entity, null);
        }

        public static string ToJson(this object entity)
        {
            return JsonSerializer.Serialize(entity, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}