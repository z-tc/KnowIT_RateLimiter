using System.Threading.Tasks;
using KnowITRateLimiter.Models;

namespace KnowITRateLimiter.Ports
{
    public interface IRateLimiterCacheProvider : ICacheProvider<RateLimiterOptions>
    {
        Task SeedIt();
    }
}
