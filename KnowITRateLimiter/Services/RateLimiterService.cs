#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowITRateLimiter.Models;
using KnowITRateLimiter.Ports;
using KnowITRateLimiter.Providers.Keys;

namespace KnowITRateLimiter.Services
{
    public class RateLimiterService : IRateLimiterService
    {
        private readonly IRateLimiterCacheProvider _rateLimiterCache;
        private readonly IClientStatisticsCacheProvider _clientStatisticsCache;

        public RateLimiterService(IRateLimiterCacheProvider rateLimiterCache, IClientStatisticsCacheProvider clientStatisticsCache)
        {
            _rateLimiterCache = rateLimiterCache;
            _clientStatisticsCache = clientStatisticsCache;
        }

        public async Task<EndpointLimit?> GetWinningRule(ClientIdentity identity,
            CancellationToken cancellationToken = default)
        {
            var cachedOptions = await _rateLimiterCache.GetAsync(new RateLimiterCacheKey(), cancellationToken);

            if (cachedOptions is not {RequestLimiterEnabled: true}) return null;

            var specificRule = cachedOptions.EndpointLimits
                .Where(x => x.Endpoint == identity.RequestedEndpoint)
                .GroupBy(x => x.RequestLimitMs)
                .Select(x => x.OrderBy(rule => rule.RequestLimitCount))
                .Select(x => x.First())
                .FirstOrDefault();

            if (specificRule == default)
            {
                return new EndpointLimit
                {
                    Endpoint = identity.RequestedEndpoint,
                    RequestLimitCount = cachedOptions.DefaultRequestLimitCount,
                    RequestLimitMs = cachedOptions.DefaultRequestLimitMs
                };
            }

            return specificRule;
        }

        public async Task<ClientStatistics?> GetClientStatistics(ClientIdentity identity, CancellationToken cancellationToken = default)
        {
            var clientCache = await _clientStatisticsCache.GetAsync(
                new ClientStatisticsCacheKey(identity), cancellationToken);

            return clientCache;
        }

        public async Task UpdateClientStatistics(ClientIdentity identity, EndpointLimit endpointLimit, CancellationToken cancellationToken = default)
        {
            var key = new ClientStatisticsCacheKey(identity);
            var clientStatistics = await _clientStatisticsCache.GetAsync(key, cancellationToken);

            if (clientStatistics != null)
            {
                clientStatistics.LastResponseTime = DateTime.UtcNow;

                if (clientStatistics.PerformedRequests == endpointLimit.RequestLimitCount)
                {
                    clientStatistics.PerformedRequests = 1;
                }
                else
                {
                    clientStatistics.PerformedRequests++;
                }
            }
            else
            {
                clientStatistics = new ClientStatistics
                {
                    LastResponseTime = DateTime.UtcNow,
                    PerformedRequests = 1
                };
            }

            await _clientStatisticsCache.SetAsync(key, clientStatistics, cancellationToken);
        }

        public async Task<bool?> EndpointRuleExist(string endpoint, CancellationToken cancellationToken)
        {
            var cachedOptions = await _rateLimiterCache.GetAsync(new RateLimiterCacheKey(), cancellationToken);

            return cachedOptions?.EndpointLimits.Any(x => x.Endpoint == endpoint);
        }
    }
}
