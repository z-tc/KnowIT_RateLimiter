#nullable enable
using System.Threading;
using System.Threading.Tasks;
using KnowITRateLimiter.Models;

namespace KnowITRateLimiter.Ports
{
    public interface IRateLimiterService
    {
        Task<EndpointLimit?> GetWinningRule(ClientIdentity identity, CancellationToken cancellationToken = default);
        Task<ClientStatistics?> GetClientStatistics(ClientIdentity identity, CancellationToken cancellationToken = default);
        Task UpdateClientStatistics(ClientIdentity identity, EndpointLimit endpointLimit, CancellationToken cancellationToken = default);
        Task<bool?> EndpointRuleExist(string endpoint, CancellationToken cancellationToken);
    }
}
