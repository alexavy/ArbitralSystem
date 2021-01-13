using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ASLogger = ArbitralSystem.Common.Logger;
#pragma warning disable 1591
namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService
{

    [UsedImplicitly]
    public class Program
    {
        private static readonly string EnvironmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string ApplicationName = "MqDistributorManager";
        

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{EnvironmentBuildName}.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            var loggerWrapper = new ASLogger.LoggerFactory(configuration).GetInstance();
            Log.Logger = loggerWrapper.GetRootLogger() as ILogger;
            loggerWrapper = loggerWrapper.ForContext("Application", ApplicationName);
            
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