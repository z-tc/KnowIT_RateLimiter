using System;
using System.Threading.Tasks;
using KnowITRateLimiter.Models;
using KnowITRateLimiter.Ports;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KnowITRateLimiter
{
    public class RateLimiterMiddleware
    {
        private const string EndpointWildcard = "*";

        private readonly RequestDelegate _next;
        private readonly RateLimiterOptions _options;
        private readonly IRateLimiterService _rateLimiterService;
        private readonly ILogger<RateLimiterMiddleware> _logger;


        public RateLimiterMiddleware(RequestDelegate next, IOptions<RateLimiterOptions> options, IRateLimiterService rateLimiterService, ILogger<RateLimiterMiddleware> logger)
        {
            _next = next;
            _options = options?.Value;
            _rateLimiterService = rateLimiterService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_options is not {RequestLimiterEnabled: true})
            {
                _logger.LogInformation("No rate limiting options provided, proceeding to next pipeline middleware...");
                await _next(context);
                return;
            }

            var clientIdentity = await GenerateClientIdentity(context);

            var winningRule = await _rateLimiterService.GetWinningRule(clientIdentity, context.RequestAborted);

            if (winningRule == null)
            {
                _logger.LogInformation("Rate limiter disabled, proceeding to next pipeline middleware...");
                await _next(context);
                return;
            }

            _logger.LogInformation($"Incoming request from client with identity => {clientIdentity}");
            _logger.LogInformation($"Checking request against rate limit rule => {winningRule}");

            var clientStatistics = await _rateLimiterService.GetClientStatistics(clientIdentity, context.RequestAborted);

            if (clientStatistics != null &&
                DateTime.UtcNow < clientStatistics.LastResponseTime.AddMilliseconds(winningRule.RequestLimitMs) &&
                clientStatistics.PerformedRequests == winningRule.RequestLimitCount)
            {
                _logger.LogError($"Request quota of {winningRule.RequestLimitCount} hits in {winningRule.RequestLimitMs}ms, " +
                                 $"exceeded for ip: {clientIdentity.IpAddress} and endpoint: {clientIdentity.RequestedEndpoint}.");

                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            await _rateLimiterService.UpdateClientStatistics(clientIdentity, winningRule);

            _logger.LogInformation("Request successfully processed!");

            await _next(context);
        }

        private async Task<ClientIdentity> GenerateClientIdentity(HttpContext context)
        {
            var endpointRuleExists = await _rateLimiterService.EndpointRuleExist(context.Request.Path, context.RequestAborted);

            string path = endpointRuleExists.HasValue && endpointRuleExists.Value ? context.Request.Path : EndpointWildcard;

            return new ClientIdentity(path, context.Connection.RemoteIpAddress.ToString());
        }
            
    }
}
