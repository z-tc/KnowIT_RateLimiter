using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using KnowITRateLimiter.Ports;
using Microsoft.Extensions.Caching.Distributed;

namespace KnowITRateLimiter.Providers.DistributedCache
{
    public class DistributedCacheProvider<T> : ICacheProvider<T>
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync(ICacheKey key, CancellationToken cancellationToken = default)
        {
            var retVal = await _cache.GetStringAsync(key.CacheKey, cancellationToken);
            return retVal == null ? default : JsonSerializer.Deserialize<T>(retVal);
        }

        public async Task SetAsync(ICacheKey key, T entry, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.MaxValue);

            var serialized = JsonSerializer.Serialize(entry);
            await _cache.SetStringAsync(key.CacheKey, serialized, options, cancellationToken);
        }
    }
}
