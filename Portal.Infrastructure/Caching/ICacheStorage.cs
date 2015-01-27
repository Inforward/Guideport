using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Text;

namespace Portal.Infrastructure.Caching
{
    public interface ICacheStorage
    {
        void Remove(string key);
        void Store(string key, object data);
        void Store(string key, object data, int cacheExpirationInSeconds);
        void ClearNamespace(string sNamespace);
        T Retrieve<T>(string storageKey);
        T Retrieve<T>(string storageKey, Func<T> fetchMethod);
    }
}
