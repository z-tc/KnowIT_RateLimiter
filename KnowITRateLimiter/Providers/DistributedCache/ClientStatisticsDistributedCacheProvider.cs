using KnowITRateLimiter.Models;
using KnowITRateLimiter.Ports;
using Microsoft.Extensions.Caching.Distributed;

namespace KnowITRateLimiter.Providers.DistributedCache
{
    public class ClientStatisticsDistributedCacheProvider : DistributedCacheProvider<ClientStatistics>, IClientStatisticsCacheProvider
    {
        public ClientStatisticsDistributedCacheProvider(IDistributedCache cache) : base(cache)
        {
        }
    }
}
