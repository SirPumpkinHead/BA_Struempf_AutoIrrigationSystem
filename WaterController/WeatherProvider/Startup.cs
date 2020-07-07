using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherProvider.Services;
using WeatherProvider.Services.Impl;

namespace WeatherProvider
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
            services.AddControllers();

            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IWeatherService, WeatherService>();
            services.AddTransient<IForecastService, ForecastService>();

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // simple request logging
            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetService<ILogger<Startup>>();

                logger.LogInformation("[{method}] {path}{queryString}", context.Request.Method, context.Request.Path,
                    context.Request.QueryString);

                await next.Invoke();

                logger.LogDebug("[{method}] {path}", context.Response.StatusCode, context.Request.Path);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}