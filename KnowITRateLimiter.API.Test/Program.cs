using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using KnowITRateLimiter.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace KnowITRateLimiter.API.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                //seed data from appsettings.json in cache on startup
                var rateLimiterProvider = scope.ServiceProvider.GetRequiredService<IRateLimiterCacheProvider>();
                await rateLimiterProvider.SeedIt();
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
