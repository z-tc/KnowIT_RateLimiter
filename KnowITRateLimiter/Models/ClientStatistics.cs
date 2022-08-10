using System;

namespace KnowITRateLimiter.Models
{
    public class ClientStatistics
    {
        public DateTime LastResponseTime { get; set; }
        public int PerformedRequests { get; set; }
    }
}
