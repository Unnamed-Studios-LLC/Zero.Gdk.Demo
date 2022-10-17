using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using UnnamedStudios.Common.DependencyInjection;
using Zero.Demo.Api.Providers;
using Zero.Demo.Api.Providers.Interfaces;
using Zero.Demo.Api.Providers.Local;
using Zero.Demo.Api.Providers.Zero;
using Zero.Demo.Core;
using Zero.ServiceApi.Model;

namespace Zero.Demo.Api
{
    public class Startup
    {
        public Startup(DemoSettings settings)
        {
            Settings = settings;
        }

        public DemoSettings Settings { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Settings);
            services.AddCommon();

            var mvc = services.AddMvc();
            if (Settings.Local)
            {
                mvc.PartManager.ApplicationParts.Clear();
            }
            mvc.AddApplicationPart(Assembly.GetExecutingAssembly());

            services.AddScoped<ICachedDeploymentProvider, CachedDeploymentProvider>();
            services.AddSingleton<GameClientProvider>();
            services.AddSingleton(new ProfanityFilter.ProfanityFilter());

            // local conditional providers
            if (Settings.Local)
            {
                services.AddTransient<IDeploymentProvider, LocalDeploymentProvider>();
            }
            else
            {
                services.AddSingleton(new ZeroServiceApiClient(Settings.ZeroServiceUrl, Settings.ZeroServiceToken));
                services.AddTransient<IDeploymentProvider, ZeroDeploymentProvider>();
            }

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
