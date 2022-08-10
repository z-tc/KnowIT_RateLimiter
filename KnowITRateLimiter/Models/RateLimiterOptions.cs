using System.Collections.Generic;

namespace KnowITRateLimiter.Models
{
    public class RateLimiterOptions
    {
        public bool RequestLimiterEnabled { get; set; }
        public int DefaultRequestLimitMs { get; set; }
        public int DefaultRequestLimitCount { get; set; }
        public List<EndpointLimit> EndpointLimits { get; set; } = new();
    }
}
