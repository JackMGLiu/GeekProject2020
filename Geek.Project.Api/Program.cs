using Autofac.Extensions.DependencyInjection;
using Geek.ProjectCore.Common.Configs;
using Geek.ProjectCore.Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Geek.Project.Api
{
    public class Program
    {
        private static AppConfig appConfig = new ConfigHelper().Get<AppConfig>("AppConfig") ?? new AppConfig();

        public static async Task<int> Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                Console.WriteLine("launching...");
                var host = CreateHostBuilder(args).Build();
                //Console.WriteLine($"{string.Join("\r\n", appConfig.Urls)}\r\n");
                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                return 1;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
             .UseServiceProviderFactory(new AutofacServiceProviderFactory())
             .ConfigureWebHostDefaults(webBuilder =>
             {

                 webBuilder
                 //.UseEnvironment(Environments.Production)
                 .UseStartup<Startup>();
             })
             .ConfigureLogging(logging =>
             {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
              })
            .UseNLog();
        }
    }
}
