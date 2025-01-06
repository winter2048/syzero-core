using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SyZero.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(conf =>
            {
                conf.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls($"{AppConfig.ServerOptions.Protocol}://{AppConfig.ServerOptions.Ip}:{AppConfig.ServerOptions.Port}")
                        .UseStartup<Startup>();
                });
    }
}
