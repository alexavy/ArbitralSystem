using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

using ASLogger = ArbitralSystem.Common.Logger;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService
{
    public class Program
    {
        private static readonly string _environmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string _applicationName = "DistributorManager";
        
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{_environmentBuildName}.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            var loggerWrapper = new ASLogger.LoggerFactory(configuration).GetInstance();
            Log.Logger = loggerWrapper.GetRootLogger() as Serilog.ILogger;
            loggerWrapper = loggerWrapper.ForContext("Application", _applicationName);

            try
            {
                CreateHostBuilder(configuration, loggerWrapper).Build().Run();
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

        private static IWebHostBuilder  CreateHostBuilder(IConfigurationRoot config, ASLogger.ILogger logger)
        {
            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseSerilog()
                .ConfigureServices(servicesCollection =>
                {
                    servicesCollection.AddSingleton<ASLogger.ILogger>(logger);
                    servicesCollection.AddSingleton<IConfigurationRoot>(config);
                })
                .UseStartup<Startup>();

        }
    }
}