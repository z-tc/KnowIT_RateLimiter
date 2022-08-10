using System.Threading;
using System.Threading.Tasks;
using KnowITRateLimiter.Ports;
using Microsoft.Extensions.Caching.Memory;

namespace KnowITRateLimiter.Providers.MemoryCache
{
    public class MemoryCacheProvider<T> : ICacheProvider<T>
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetAsync(ICacheKey key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_cache.TryGetValue(key.CacheKey, out T stored) ? stored : default);
        }

        public Task SetAsync(ICacheKey key, T entry, CancellationToken cancellationToken = default)
        {
            var options = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            };

            _cache.Set(key.CacheKey, entry, options);

            return Task.CompletedTask;
        }
    }
}
