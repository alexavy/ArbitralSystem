using System;
using System.IO;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Options;
using ArbitralSystem.Service.Core.Messaging;
using ArbitralSystem.Storage.FakeMarketInfoStorageService.Common;
using ArbitralSystem.Storage.FakeMarketInfoStorageService.Consumers;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Storage.FakeMarketInfoStorageService
{
    internal class Program
    {
        private static readonly string EnvironmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string ApplicationName = "FakeMarketInfoStorage";

        static async Task Main()
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
                await FakeMarketInfoStorageService(configuration, loggerWrapper);
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

        static async Task FakeMarketInfoStorageService(IConfigurationRoot configuration, ILogger loggerWrapper)
        {
            var mqOptions = configuration.GetSection("RabbitMQ:Connection")
                .Get<ConnectionOptions>();

            var serviceStorageOptions = configuration.GetSection("MarketInfoStorageService:Options")
                .Get<ServiceStorageOptions>();

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) => { configApp.AddConfiguration(configuration); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(loggerWrapper);
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<FakeOrderBookConsumer>();
                        x.AddConsumer<FakeDistributerStateConsumer>();
                        MessageCorrelation.UseCorrelationId<IOrderBookPackageMessage>(o => o.CorrelationId);
                        MessageCorrelation.UseCorrelationId<IDistributerStateMessage>(o => o.CorrelationId);

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            cfg.Host(new Uri(mqOptions.Host), h => { });

                            cfg.ReceiveEndpoint(Constants.Queues.OrderbooksStorageConsumer, e =>
                            {
                                e.PrefetchCount = serviceStorageOptions.PrefetchCount;
                                e.ConfigureConsumer<FakeOrderBookConsumer>(provider);
                            });

                            cfg.ReceiveEndpoint(Constants.Queues.DistributerConnectionStatesConsumer, e =>
                            {
                                e.PrefetchCount = 1;
                                e.ConfigureConsumer<FakeDistributerStateConsumer>(provider);
                            });
                        }));
                    });
                    //services.AddHostedService<StorageService>();
                    services.AddHostedService<BusService>();
                }).UseConsoleLifetime();
            await hostBuilder.RunConsoleAsync();
        }
    }
}