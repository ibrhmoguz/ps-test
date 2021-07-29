using Microsoft.Extensions.Caching.Memory;
using Paymentsense.Coding.Challenge.Api.Interfaces;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public class SimpleCacheService<T> : ISimpleCache<T>
    {
        private readonly IMemoryCache _memoryCache;

        public SimpleCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get(object key)
        {
            _memoryCache.TryGetValue(key, out T entry);
            return entry;
        }

        public bool Set(object key, T entry)
        {
            if (_memoryCache.TryGetValue(key, out _))
            {
                return false;
            }

            _memoryCache.Set(key, entry);
            return true;
        }
    }
}
