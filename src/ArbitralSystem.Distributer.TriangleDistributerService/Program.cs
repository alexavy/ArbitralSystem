using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Distributer.PairPolygonDistributerService.Options;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Options;
using ArbitralSystem.Service.Core;
using ArbitralSystem.Storage.RemoteCacheStorage;
using ArbitralSystem.Storage.RemoteCacheStorage.Options;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Service.Core.Messaging;

namespace ArbitralSystem.Distributer.PairPolygonDistributerService
{
    class Program
    {
        private static string _environmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static string _applicationName = "TriangleDistibuter";
        static async Task Main(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", false)
                                .AddJsonFile($"appsettings.{_environmentBuildName}.json", true)
                                .AddEnvironmentVariables()
                                .Build();

            ITriangleDistributerOptions distributerOptions = configuration.GetSection("TriangleDistibuter:Options")
                .Get<TriangleDistributerOptions>();

            ICacheStorageOptions storageOptions = configuration.GetSection("Cache:Connection")
                .Get<CacheStorageOptions>();

            IExchangeConnectionInfo[] ConnectionInfoArray = new IExchangeConnectionInfo[]
            { configuration.GetSection("Connectors:CoinEx").Get<ExchangeConnectionInfo>() };

            IConnectionOptions mqOptions = configuration.GetSection("RabbitMQ:Connection").Get<ConnectionOptions>();

            var loggerWrapper = new LoggerFactory(configuration).GetInstance();
            loggerWrapper = loggerWrapper.ForContext("Application", _applicationName);

            var hostBuilder = new HostBuilder()
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                configApp.AddConfiguration(configuration);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILogger>(loggerWrapper);
                services.AddSingleton<ICacheStorageOptions>(storageOptions);
                services.AddSingleton<IExchangeConnectionInfo[]>(ConnectionInfoArray);

                
                services.AddTransient<IExtendedExchangeConnector, ExtendedExchangeConnector>();

                services.AddSingleton<IPairCacheStorage, PairCacheStorageRedisClient>();
                services.AddSingleton<IRedisWrapper, RedisWrapper>();
                services.AddSingleton<IDtoConverter, CryptoExchangeConverter>();
                services.AddTransient<ICoinExConnector, CoinExConnector>();
                services.AddSingleton<IPublicConnectorFactory, CryptoExPublicConnectorFactory>();

              /*  services.AddMassTransit(x =>
                {
                    MessageCorrelation.UseCorrelationId<IPairPolygonMessage>(o => o.CorrelationId);
                    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri(mqOptions.Host), h => { });

                        cfg.ReceiveEndpoint(Constants.Queues.PairPolygons, e =>
                        {
                            EndpointConvention.Map<IPairPolygonMessage>(e.InputAddress);
                        });
                    }));
                });*/
                services.AddSingleton<IHostedService, BusService>();
                services.AddSingleton<IDomainBusProducer, DomainBusProducer>();

                services.AddHostedService<TriangleService>();
            })
            .UseConsoleLifetime();

            await hostBuilder.RunConsoleAsync();
        }
    }
}
