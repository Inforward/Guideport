using System;
using System.Web;

namespace Portal.Infrastructure.Helpers
{
    public static class SessionHelper
    {
        public static void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static T Get<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrWhiteSpace(key))
                return defaultValue;

            var value = HttpContext.Current.Session[key];

            if (value == null)
                return default(T);

            var type = typeof(T);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(typeof(T));

            return (T)Convert.ChangeType(value, type);
        }
    }
}