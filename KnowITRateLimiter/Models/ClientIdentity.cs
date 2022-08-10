namespace KnowITRateLimiter.Models
{
    public class ClientIdentity
    {
        public ClientIdentity(string requestedEndpoint, string ipAddress)
        {
            RequestedEndpoint = requestedEndpoint;
            IpAddress = ipAddress;
        }

        public string IpAddress { get; }
        public string RequestedEndpoint { get; }

        public override string ToString()
        {
            return $"RequestedEndpoint: {RequestedEndpoint}, IpAddress: {IpAddress}";
        }
    }
}
