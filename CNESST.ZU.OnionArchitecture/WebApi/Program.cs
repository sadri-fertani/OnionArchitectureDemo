using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((hostingContext, loggerConfiguration) =>
                {
                    // clear default logging providers
                    loggerConfiguration.ClearProviders();
                    loggerConfiguration.AddEventLog(new EventLogSettings()
                    {
                        SourceName = "CNESST_API_DEMO",
                        LogName = "CNESST_API_DEMO",
                        Filter = (x, y) => y >= LogLevel.Information
                    });
                });
    }
}
