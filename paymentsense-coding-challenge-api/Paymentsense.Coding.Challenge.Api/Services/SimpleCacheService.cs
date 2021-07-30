using Microsoft.Extensions.Caching.Memory;
using Paymentsense.Coding.Challenge.Api.Interfaces;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public class SimpleCacheService<T> : ISimpleCache<T>
    {
        private static readonly IMemoryCache MemoryCache = new MemoryCache(new MemoryCacheOptions());

        public SimpleCacheService()
        {
        }

        public T Get(object key)
        {
            MemoryCache.TryGetValue(key, out T entry);
            return entry;
        }

        public bool Set(object key, T entry)
        {
            if (MemoryCache.TryGetValue(key, out _))
            {
                return false;
            }

            MemoryCache.Set(key, entry);
            return true;
        }
    }
}
