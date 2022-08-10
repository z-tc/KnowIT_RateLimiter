## KnowIT Rate Limiter
**.NET Core** `RateLimiter` library that implements the basic functionality of filtering access to service endpoints based on configurable limits.

## Setup and configuration

**Startup.cs**
```cs
public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //load config from appsettings.json
            services.AddOptions();
            //cache options and client statistics in memory
            services.AddMemoryCache();
            //services.AddDistributedMemoryCache();
            //map configuration to RateLimiterOptions
            services.Configure<RateLimiterOptions>(Configuration.GetSection("RateLimiter"));
            //setup dependency for memory/distributed cache rate limiting
            services.AddInMemoryRateLimiting();
            //services.AddDistributedRateLimiting();
            
            services.AddControllers();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //use rate limiting middleware
            app.UseRateLimiting();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
```
If you need to scale and load-balance your app, you'll need to use distributed cache, otherwise memory cache can be used.

**Program.cs**
```cs
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
```

**Configuration**
```
| Parameter                         | Description                                                           |   Type    |
| --------------------------------- | --------------------------------------------------------------------- | --------- |
| RequestLimiterEnabled             | Toggles rate limiting                                                 |  boolean  |
| DefaultRequestLimitMs             | Default request limit - applied when there is no specific endpoint    |  integer  |
|                                   | rate configured                                                       |           |
| DefaultRequestLimitCount          | Default limit for sequential requested in default timespan            |  integer  |
| EndpointLimits                    | Specific endpoint rate limit config                                   |           |
| EndpointLimits/Endpoint           | Path                                                                  |  string   |
| EndpointLimits/RequestLimitMs     | Request limit in milliseconds                                         |  integer  |
| EndpointLimits/RequestLimitCount  | Limit for sequential requests in defined timespan                     |  integer  |
```
**appsettings.json:**

```json
{
    "RateLimiter": {
    "RequestLimiterEnabled": true,
    "DefaultRequestLimitMs": 1000,
    "DefaultRequestLimitCount": 2,
    "EndpointLimits": [
      {
        "Endpoint": "/app",
        "RequestLimitMs": 10000,
        "RequestLimitCount": 2
      },
      {
        "Endpoint": "/weatherforecast",
        "RequestLimitMs": 10000,
        "RequestLimitCount": 5
      }
    ]
  }
}