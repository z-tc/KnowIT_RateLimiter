using KnowITRateLimiter.Models;
using KnowITRateLimiter.Ports;

namespace KnowITRateLimiter.Providers.Keys
{
    public class ClientStatisticsCacheKey : ICacheKey
    {
        private readonly string _endpoint;
        private readonly string _ipAddress;

        public ClientStatisticsCacheKey(string endpoint, string ipAddress)
        {
            _endpoint = endpoint;
            _ipAddress = ipAddress;
     
        }
        public ClientStatisticsCacheKey(ClientIdentity identity)
        {
            _endpoint = identity.RequestedEndpoint;
            _ipAddress = identity.IpAddress;

        }

        public string CacheKey => $"ClientId_{_endpoint}_{_ipAddress}";
    }
}
