using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using UnnamedStudios.Common.AspNetCore;
using Zero.Demo.Core;
using Zero.Service.Model;

namespace Zero.Demo.Api
{
    public static class Extensions
    {
        public static void ConfigureDemoHttps(this IWebHostBuilder webBuilder, DemoSettings settings)
        {
            webBuilder.ConfigureHttps((context, configureAction) =>
            {
                if (context.HostingEnvironment.IsProduction())
                {
                    var certificate = ZeroCertificate.Load(settings.HttpsCertificatePassword);
                    configureAction(443, certificate);
                    configureAction(80, null);
                }
            });
        }
    }
}
