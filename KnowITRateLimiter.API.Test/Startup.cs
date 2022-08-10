using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KnowITRateLimiter.Models;
using KnowITRateLimiter.Utils;

namespace KnowITRateLimiter.API.Test
{
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
}
