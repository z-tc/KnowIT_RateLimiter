using System.Threading.Tasks;
using KnowITRateLimiter.Models;
using KnowITRateLimiter.Ports;
using KnowITRateLimiter.Providers.Keys;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace KnowITRateLimiter.Providers.DistributedCache
{
    public class RateLimiterDistributedCacheProvider : DistributedCacheProvider<RateLimiterOptions>, IRateLimiterCacheProvider
    {
        private readonly RateLimiterOptions _options;

        public RateLimiterDistributedCacheProvider(
            IDistributedCache cache,
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
