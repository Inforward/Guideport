using System;

namespace Portal.Infrastructure.Helpers
{
    public static class Utilities
    {
        public static T Coalesce<T>(params Func<T>[] methods)
        {
            T value = default(T);

            foreach (var method in methods)
            {
                value = method();

                if (value != null)
                    break;
            }

            return value;
        }   
    }
}
