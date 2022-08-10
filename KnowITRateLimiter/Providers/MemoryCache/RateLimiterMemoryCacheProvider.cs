using System.Threading.Tasks;
using KnowITRateLimiter.Models;
using KnowITRateLimiter.Ports;
using KnowITRateLimiter.Providers.Keys;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace KnowITRateLimiter.Providers.MemoryCache
{
    public class RateLimiterMemoryCacheProvider : MemoryCacheProvider<RateLimiterOptions>, IRateLimiterCacheProvider
    {
        private readonly RateLimiterOptions _options;

        public RateLimiterMemoryCacheProvider(
            IMemoryCache cache,
            IOptions<RateLimiterOptions> options) : base(cache)
        {
            _options = options?.Value;
        }

        public async Task SeedIt()
        {
            if (_options != null)
            {
                var key = new RateLimiterCacheKey();
                await SetAsync(key, _options).ConfigureAwait(false);
            }
        }
    }

    
}
