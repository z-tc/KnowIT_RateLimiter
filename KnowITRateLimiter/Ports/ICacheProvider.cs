using System.Threading;
using System.Threading.Tasks;

namespace KnowITRateLimiter.Ports
{
    public interface ICacheProvider<T>
    {
        Task<T> GetAsync(ICacheKey key, CancellationToken cancellationToken = default);
        Task SetAsync(ICacheKey key, T entry, CancellationToken cancellationToken = default);
    }
}
