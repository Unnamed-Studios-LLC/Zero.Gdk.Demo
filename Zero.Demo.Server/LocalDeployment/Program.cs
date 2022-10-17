using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Zero.Demo.Api;
using Zero.Demo.Core;
using Zero.Demo.World;
using Zero.Game.Local;
using Zero.Service.Model;

namespace LocalDeployment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var settings = ZeroConfiguration.GetConfiguration<DemoSettings>();
            var apiTask = CreateHostBuilder(args, settings).Build().RunAsync();
            var worldTask = ZeroLocal.RunAsync<DemoServerPlugin>(args);

            await Task.WhenAll(worldTask, apiTask);
        }

        public static IHostBuilder CreateHostBuilder(string[] args, DemoSettings settings) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(x => new Startup(settings));
                })
                .ConfigureLogging(logging => logging.ClearProviders());
    }
}
