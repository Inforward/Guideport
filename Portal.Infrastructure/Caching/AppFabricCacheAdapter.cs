using Microsoft.ApplicationServer.Caching;
using Portal.Infrastructure.Configuration;
using System;
using System.Linq;

namespace Portal.Infrastructure.Caching
{
    public class AppFabricCacheAdapter : ICacheStorage
    {
        private static readonly object PadLock = new object();
        private static readonly DataCacheFactory Factory = new DataCacheFactory();
        private static readonly DataCache Cache = Factory.GetCache(Settings.Get<string>("app:AppFabric.CacheName"));

        public void Remove(string key)
        {
            lock (PadLock)
            {
                Cache.Remove(key);
            }
        }

        public void Store(string key, object data)
        {
            Cache.Add(key, data);
        }

        public void Store(string key, object data, int cacheExpirationInSeconds)
        {
            Cache.Add(key, data, TimeSpan.FromSeconds(cacheExpirationInSeconds));
        }

        public void ClearNamespace(string sNamespace)
        {
            foreach (var region in Cache.GetSystemRegions())
            {
                var keys = Cache.GetObjectsInRegion(region)
                                .Select(kvp => kvp.Key)
                                .Where(key => key.StartsWith(sNamespace, StringComparison.InvariantCultureIgnoreCase));

                foreach (var key in keys)
                {
                    Remove(key);
                }
            }
        }

        public T Retrieve<T>(string key)
        {
            return (T) Cache.Get(key);
        }

        public T Retrieve<T>(string key, Func<T> fetchMethod)
        {
            var result = Retrieve<T>(key);

            if (result == null)
            {
                lock (PadLock)
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
