using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebStore.Extensions
{
    public static class SessionExtension
    {
        // Get dynamyc typed GET method (from JSON)
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        // Add dynamic typed SET method (to JSON)
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}