using KnowITRateLimiter.Ports;
using KnowITRateLimiter.Providers.DistributedCache;
using KnowITRateLimiter.Providers.MemoryCache;
using KnowITRateLimiter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KnowITRateLimiter.Utils
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddInMemoryRateLimiting(this IServiceCollection services)
        {
            services.AddSingleton<IRateLimiterService, RateLimiterService>();
            services.AddSingleton<IRateLimiterCacheProvider, RateLimiterMemoryCacheProvider>();
            services.AddSingleton<IClientStatisticsCacheProvider, ClientStatisticsMemoryCacheProvider>();
            return services;
        }

        public static IServiceCollection AddDistributedRateLimiting(this IServiceCollection services)
        {
            services.AddSingleton<IRateLimiterService, RateLimiterService>();
            services.AddSingleton<IRateLimiterCacheProvider, RateLimiterDistributedCacheProvider>();
            services.AddSingleton<IClientStatisticsCacheProvider, ClientStatisticsDistributedCacheProvider>();
            return services;
        }

        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}