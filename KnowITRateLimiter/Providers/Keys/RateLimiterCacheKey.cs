using KnowITRateLimiter.Ports;

namespace KnowITRateLimiter.Providers.Keys
{
    class RateLimiterCacheKey : ICacheKey
    {
        public string CacheKey => "Limiter";
    }
}
