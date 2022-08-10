using KnowITRateLimiter.Models;
using KnowITRateLimiter.Ports;
using Microsoft.Extensions.Caching.Memory;

namespace KnowITRateLimiter.Providers.MemoryCache
{
    public class ClientStatisticsMemoryCacheProvider : MemoryCacheProvider<ClientStatistics>, IClientStatisticsCacheProvider
    {
        public ClientStatisticsMemoryCacheProvider(IMemoryCache cache) : base(cache)
        {
        }
    }
}
