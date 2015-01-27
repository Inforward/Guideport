using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Portal.Infrastructure.Caching
{
    public class HttpContextCacheAdapter : ICacheStorage
    {
        private static readonly object padLock = new object();

        public void Remove(string key)
        {
            lock (padLock)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        public void Store(string key, object data)
        {
            HttpRuntime.Cache.Insert(key, data);
        }

        public void Store(string key, object data, int cacheExpirationInSeconds)
        {
            HttpRuntime.Cache.Insert(key, data, null, DateTime.Now.AddSeconds(cacheExpirationInSeconds), Cache.NoSlidingExpiration);
        }

        public void ClearNamespace(string sNamespace)
        {
            lock (padLock)
            {
                var keys = HttpRuntime.Cache.Cast<DictionaryEntry>()
                                .Select(entry => entry.Key.ToString())
                                .Where(key => key.StartsWith(sNamespace, StringComparison.InvariantCultureIgnoreCase));

                foreach (var key in keys)
                {
                    Remove(key);
                }
            }
        }

        public T Retrieve<T>(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }

        public T Retrieve<T>(string key, Func<T> fetchMethod)
        {
            var result = Retrieve<T>(key);

            if (result == null)
            {
                lock (padLock)
                {
                    result = Retrieve<T>(key);

                    if (result == null)
                    {
                        result = fetchMethod();

                        if (result != null)
                            Store(key, result);
                    }
                }
            }

            return result;
        }
    }
}
