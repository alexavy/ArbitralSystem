using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DistributorManagementService
{
    public class Program
    {
        private static readonly string _environmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string _applicationName = "DistributerControlService";
        
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{_environmentBuildName}.json", true)
                .AddEnvironmentVariables()
                .Build();
            
            var loggerWrapper = new ArbitralSystem.Common.Logger.LoggerFactory(configuration).GetInstance();
            Log.Logger = loggerWrapper.GetRootLogger() as Serilog.ILogger;
            loggerWrapper = loggerWrapper.ForContext("Application", _applicationName);

            try
            {
                CreateHostBuilder(args, loggerWrapper).Build().Run();
            }
            catch (Exception e)
            {
                loggerWrapper.Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                loggerWrapper.Dispose();
            }
        }

        private static IHostBuilder  CreateHostBuilder(string[] args, ArbitralSystem.Common.Logger.ILogger logger)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices(servicesCollection => { servicesCollection.AddSingleton<ArbitralSystem.Common.Logger.ILogger>(logger); })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        }
    }
}