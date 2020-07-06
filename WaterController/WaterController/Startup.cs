using ContextBrokerLibrary.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WaterController.Services;
using WaterController.Services.Impl;

namespace WaterController
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
            // Setting default headers for all requests to context broker
            ContextBrokerLibrary.Client.Configuration.Default.AddDefaultHeader("fiware-service",
                Configuration.GetValue<string>("FiwareService"));
            ContextBrokerLibrary.Client.Configuration.Default.AddDefaultHeader("fiware-servicepath",
                Configuration.GetValue<string>("FiwareServicePath"));
            ContextBrokerLibrary.Client.Configuration.Default.BasePath = GetBasePath();

            services.AddControllers();

            services.AddTransient<IEntityService, EntityService>();

            services.AddSingleton<IEntitiesApi>(new EntitiesApi(ContextBrokerLibrary.Client.Configuration.Default));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private string GetBasePath()
        {
            return Configuration.GetValue<string>("ContextBrokerPath", null);
        }
    }
}