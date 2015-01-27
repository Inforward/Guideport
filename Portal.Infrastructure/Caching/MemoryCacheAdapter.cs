using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using System;

namespace Portal.Infrastructure.Caching
{
    public class MemoryCacheAdapter : ICacheStorage
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly int _cacheExpirationInSeconds;
        private static readonly object PadLock = new object();

        public MemoryCacheAdapter()
        {
            
        }

        public MemoryCacheAdapter(int cacheExpirationInSeconds)
        {
            _cacheExpirationInSeconds = cacheExpirationInSeconds;
        }

        public void Remove(string key)
        {
            if (_cache.Contains(key))
            {
                _cache.Remove(key);
            }
        }

        public void Store(string key, object data, int cacheExpirationInSeconds)
        {
            var cacheItemPolicy = new CacheItemPolicy()
            {
                Priority = CacheItemPriority.Default,
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds((double)cacheExpirationInSeconds),
            };

            _cache.Set(key, data, cacheItemPolicy);
        }

        public void Store(string key, object data)
        {
            Store(key, data, _cacheExpirationInSeconds);
        }

        public void ClearNamespace(string sNamespace)
        {
            lock (PadLock)
            {
                foreach (var key in _cache.Select(kvp => kvp.Key).Where(k => k.StartsWith(sNamespace, StringComparison.InvariantCultureIgnoreCase)))
                {
                    Remove(key);
                }
            }            
        }

        public T Retrieve<T>(string key)
        {
            return (T)_cache[key];
        }

        public T Retrieve<T>(string key, Func<T> fetchMethod)
        {
            var result = Retrieve<T>(key);

            if (result == null)
            {
                result = fetchMethod();

                if (result != null)
                    Store(key, result);
            }

            return result;
        }
    }
}
