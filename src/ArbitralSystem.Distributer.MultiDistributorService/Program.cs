using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributorService
{
    
    public class Program
    {
        private static readonly string _environmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string _applicationName = "MultiDistributorService";
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{_environmentBuildName}.json", true)
                .AddEnvironmentVariables()
                .Build();
            
            var loggerWrapper = new Common.Logger.LoggerFactory(configuration).GetInstance();
            Log.Logger = loggerWrapper.GetRootLogger() as Serilog.ILogger;
            loggerWrapper = loggerWrapper.ForContext("Application", _applicationName);
            
            try
            {
                CreateHostBuilder(args, loggerWrapper).Build().Run();
            }
            catch (Exception e)
            {
                loggerWrapper.Fatal(e,"Host terminated unexpectedly");
            }
            finally
            {
                loggerWrapper.Dispose();
            }
        }

        private static IHostBuilder  CreateHostBuilder(string[] args, Common.Logger.ILogger logger)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices(servicesCollection => { servicesCollection.AddSingleton<Common.Logger.ILogger>(logger); })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        }
    }
}