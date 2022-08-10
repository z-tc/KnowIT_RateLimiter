namespace KnowITRateLimiter.Models
{
    public class EndpointLimit
    {
        public string Endpoint { get; init; }
        public int RequestLimitMs { get; init; }
        public int RequestLimitCount { get; init; }

        public override string ToString()
        {
            return $"Endpoint: {Endpoint}, RequestLimitMs: {RequestLimitMs}, RequestLimitCount: {RequestLimitCount}";
        }
    }
}
