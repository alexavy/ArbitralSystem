using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Jobs;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Messaging;
using ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Options;
using ArbitralSystem.Service.Core;
using AutoMapper;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService
{
    class Program
    {
        private static readonly string EnvironmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string ApplicationName = "OrderBookDistributer";

        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{EnvironmentBuildName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var loggerWrapper = new LoggerFactory(configuration).GetInstance();
            loggerWrapper = loggerWrapper.ForContext("Application", ApplicationName);

            try
            {
                await OrderBookDistributerService(args, configuration, loggerWrapper);
            }
            catch (OperationCanceledException)
            {
                loggerWrapper.Warning("Operation was canceled");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                loggerWrapper.Fatal(e, "Unexpected fatal error!");
                throw;
            }
            finally
            {
                loggerWrapper.Dispose();
            }
        }

        static async Task OrderBookDistributerService(string[] args, IConfigurationRoot configuration, ILogger loggerWrapper)
        {
            var serviceOptions = configuration.GetSection("ServiceOptions").Get<DistributorServiceOptions>();
            IDistributerOptions distributerOptions = configuration.GetSection("OrderBookDistributer:Options")
                .Get<DistributerOptions>();

            var settingsLogger = new SettingsLogger(loggerWrapper);


            var serverOptions = new ServerOptions(string.Concat(DistributorConstants.OrderBookDistributorIdentity, '-', serviceOptions.ServerId),
                string.Concat(DistributorConstants.OrderBookDistributorIdentity, "-queue-", serviceOptions.ServerId));

            loggerWrapper.Information($"Server: {serverOptions.ServerName}, Queue: {serverOptions.ServerQueueName}");

            GlobalConfiguration.Configuration.UseSqlServerStorage(serviceOptions.DatabaseConnectionString);
            //GlobalConfiguration.Configuration.UseActivator(new AspNetCoreJobActivator())
            
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) => { configApp.AddConfiguration(configuration); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAutoMapper(Assembly.GetExecutingAssembly(),
                        AppDomain.CurrentDomain.Load("ArbitralSystem.Distributor.ScheduleDistributor.Domain"));

                    services.AddSingleton(loggerWrapper);
                    services.AddSingleton(serverOptions);
                    services.AddSingleton(distributerOptions);
                    services.AddSingleton(new DistributionOptions(serviceOptions.Trim));
                    
                    services.AddTransient<OrderBookDistributorJob>();
                    services.AddTransient<IOrderBookPublisher, OrderBookPublisher>();

                    services.AddSingleton<IDtoConverter, CryptoExchangeConverter>();
                    services.AddTransient<IOrderBookDistributerFactory, CryptoExOrderBookDistributerFactory>();

                    /*services.AddHangfire((provider, conf) =>
                    {
                        conf.UseSqlServerStorage(serviceOptions.DatabaseConnectionString);
                        conf.UseSerilogLogProvider();
                        conf.UseFilter(new LogEverythingAttribute(loggerWrapper));
                        /*  configuration.UseFilter(new SkipConcurrentExecutionAttribute(360, loggerWrapper));
                          //configuration.UseFilter(new PreserveOriginalQueueAttribute(serverOptions));
                          configuration.UseFilter(new AutomaticRetryAttribute()
                          {
                              Attempts = int.MaxValue,
                              DelayInSecondsByAttemptFunc = (x) => 3
                          });*//*
                    });

                    services.AddHangfireServer(serverOpt =>
                    {
                        //serverOpt.StopTimeout = Timeout.InfiniteTimeSpan;
                        //serverOpt.Queues = new[] {serverOptions.ServerQueueName};
                        serverOpt.Queues = new[] {"orderbook-queue"};
                        serverOpt.ServerName = serverOptions.ServerName;
                        //serverOpt.WorkerCount = 100;
                        serverOpt.WorkerCount = serviceOptions.MaxWorkersCount;
                    });*/


                    services.AddMassTransit(x =>
                    {
                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg => { cfg.Host(new Uri(serviceOptions.MqConnectionString), h => { }); }));
                    });

                    services.AddHostedService<OrderBookDistributerService>();
                }).UseConsoleLifetime();


           // using (var server = new BackgroundJobServer(new BackgroundJobServerOptions()
           // {
           //     WorkerCount = 1,
           //     Queues = new[] {"order-book-distributor"},
           //     ServerName = serverOptions.ServerName
           // }))
           /// {
                
                await hostBuilder.RunConsoleAsync();
            //}
            
        }
    }
}