using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Distributer.PairInfoDistributerService.Options;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Options;
using ArbitralSystem.Service.Core;
using ArbitralSystem.Service.Core.Messaging;
using ArbitralSystem.Storage.RemoteCacheStorage;
using ArbitralSystem.Storage.RemoteCacheStorage.Options;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Distributer.PairInfoDistributerService
{
    internal class Program
    {
        private static readonly string _environmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string _applicationName = "PairInfoDistributer";

        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{_environmentBuildName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var loggerWrapper = new LoggerFactory(configuration).GetInstance();
            loggerWrapper = loggerWrapper.ForContext("Application", _applicationName);
            
            try
            {
                await PairInfoService(args, configuration, loggerWrapper);
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

        static async Task PairInfoService(string[] args, IConfigurationRoot configuration, ILogger loggerWrapper)
        {
             IPairInfoDistributerOptions distributerOptions = configuration.GetSection("PairInfoDistributer:Options")
                .Get<PairInfoDistributerOptions>();

            IExchangeConnectionInfo[] connectionInfoArray =
                {configuration.GetSection("Connectors:CoinEx").Get<ExchangeConnectionInfo>()};

            IConnectionOptions mqOptions = configuration.GetSection("RabbitMQ:Connection").Get<ConnectionOptions>();

            ICacheStorageOptions storageOptions =
                configuration.GetSection("Cache:Connection").Get<CacheStorageOptions>();
            
            var settingsLogger = new SettingsLogger(loggerWrapper);

            var hideRule = new PasswordRegexRule()
            {
                RegexPattern = RegexPasswordPatterns.RabbitMq,
                NameOfProperty = nameof(mqOptions.Host),
                ObjectType = mqOptions.GetType()
            };
            var passwordHideRules = new Dictionary<Type, PasswordRegexRule>();
            passwordHideRules.Add(hideRule.ObjectType,hideRule);

            settingsLogger.PasswordHiddenInfo(passwordHideRules,
                mqOptions,
                storageOptions,
                distributerOptions);
            
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) => { configApp.AddConfiguration(configuration); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(loggerWrapper);
                    services.AddSingleton(storageOptions);
                    services.AddSingleton(connectionInfoArray);
                    services.AddSingleton(distributerOptions);

                    services.AddSingleton<IPairCacheStorage, PairCacheStorageRedisClient>();
                    services.AddSingleton<IRedisWrapper, RedisWrapper>();
                    services.AddSingleton<IDtoConverter, CryptoExchangeConverter>();
                    services.AddTransient<ICoinExConnector, CoinExConnector>();
                    services.AddSingleton<IPublicConnectorFactory, CryptoExPublicConnectorFactory>();

                    services.AddMassTransit(x =>
                    {
                        MessageCorrelation.UseCorrelationId<IPairInfoMessage>(o => o.CorrelationId);
                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            var host = cfg.Host(new Uri(mqOptions.Host), h => { });

                            cfg.ReceiveEndpoint(Constants.Queues.DistributerPairs,
                                e => { EndpointConvention.Map<IPairInfoMessage>(e.InputAddress); });
                        }));
                    });
                    services.AddSingleton<IHostedService, BusService>();
                    services.AddSingleton<IDomainBusProducer, DomainBusProducer>();

                    services.AddHostedService<ControlService>();
                })
                .UseConsoleLifetime();

            await hostBuilder.RunConsoleAsync();
        }

    }
}