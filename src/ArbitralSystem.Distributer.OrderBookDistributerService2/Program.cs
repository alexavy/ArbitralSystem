using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Distributer.OrderBookDistributerService.Actions;
using ArbitralSystem.Distributer.OrderBookDistributerService.Common;
using ArbitralSystem.Distributer.OrderBookDistributerService.Consumers;
using ArbitralSystem.Distributer.OrderBookDistributerService.Models;
using ArbitralSystem.Distributer.OrderBookDistributerService.Workflow;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Options;
using ArbitralSystem.Service.Core;
using ArbitralSystem.Service.Core.Messaging;
using ArbitralSystem.Storage.RemoteCacheStorage;
using ArbitralSystem.Storage.RemoteCacheStorage.Options;
using GreenPipes;
using JetBrains.Annotations;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Distributer.OrderBookDistributerService
{
    [UsedImplicitly]
    internal class Program
    {
        private static readonly string _environmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string _applicationName = "OrderBookDistributer";
        private static readonly string _botPatternName = "OBD-";
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
                await OrderBookDistributerService(args, configuration, loggerWrapper);
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
            IExchangeConnectionInfo[] connectionInfoArray =
                {configuration.GetSection("Connectors:CoinEx").Get<ExchangeConnectionInfo>()};

            ICacheStorageOptions storageOptions = configuration.GetSection("Cache:Connection")
                .Get<CacheStorageOptions>();

            IConnectionOptions mqOptions = configuration.GetSection("RabbitMQ:Connection")
                .Get<ConnectionOptions>();

            IDistributerOptions distributerOptions = configuration.GetSection("OrderBookDistributer:Options")
                .Get<DistributerOptions>();

            IServiceDistributionOptions serviceDistributionOptions = configuration
                .GetSection("OrderBookDistributer:ServiceOptions")
                .Get<ServiceDistributionOptions>();

            var botId = Guid.NewGuid();
            var botMetaData = new BotMetaData()
            {
                BotId = botId,
                BotName = $"{_botPatternName}{botId}",
                BotState = DistributorState.Initialization
            };
            var settingsLogger = new SettingsLogger(loggerWrapper);

            var hideRule = new PasswordRegexRule()
            {
                RegexPattern = RegexPasswordPatterns.RabbitMq,
                NameOfProperty = nameof(mqOptions.Host),
                ObjectType = mqOptions.GetType()
            };
            var passwordHideRules = new Dictionary<Type, PasswordRegexRule>();
            passwordHideRules.Add(hideRule.ObjectType, hideRule);

            settingsLogger.PasswordHiddenInfo(passwordHideRules,
                storageOptions,
                mqOptions,
                distributerOptions,
                serviceDistributionOptions);

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) => { configApp.AddConfiguration(configuration); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(loggerWrapper);
                    services.AddSingleton(storageOptions);
                    services.AddSingleton(distributerOptions);
                    services.AddSingleton(serviceDistributionOptions);
                    services.AddSingleton(connectionInfoArray);

                    services.AddSingleton<IDtoConverter, CryptoExchangeConverter>();
                    services.AddSingleton<IRedisWrapper, RedisWrapper>();
                    services.AddSingleton<IPairCacheStorage, PairCacheStorageRedisClient>();

                    services.AddTransient<ICoinExConnector, CoinExConnector>();

                    services.AddSingleton<IPublicConnectorFactory, CryptoExPublicConnectorFactory>();
                    services.AddTransient<IOrderBookDistributerFactory, CryptoExOrderBookDistributerFactory>();


                    services.AddTransient<IExtendedExchangeConnector, ExtendedExchangeConnector>();
                    services.AddSingleton<IOrderBookDistributerWorkflow, OrderBookDistributerWorkflow>();

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<ExchangePairInfoConsumer>();
                        MessageCorrelation.UseCorrelationId<IExchangePairInfoMessage>(o => o.CorrelationId);
                        MessageCorrelation.UseCorrelationId<IOrderBookMessage>(o => o.CorrelationId);
                        MessageCorrelation.UseCorrelationId<IDistributerStateMessage>(o => o.CorrelationId);

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            var host = cfg.Host(new Uri(mqOptions.Host), h => { });
                            cfg.UseMessageScheduler(new Uri($"{new Uri(mqOptions.Host)}/{mqOptions.QuartzQueueName}"));

                            cfg.ReceiveEndpoint(Constants.Queues.DistributerPairsConsumer, e =>
                            {
                                e.PrefetchCount = 1;
                                e.UseMessageRetry(r => r.Immediate(serviceDistributionOptions.PairRetryCount));
                                e.ConfigureConsumer<ExchangePairInfoConsumer>(provider);
                            });

                            cfg.ReceiveEndpoint(Constants.Queues.OrderBookAggregator,
                                e => { EndpointConvention.Map<OrderBookMessage>(e.InputAddress); });

                            cfg.ReceiveEndpoint(Constants.Queues.DistributerConnectionStates,
                                e => { EndpointConvention.Map<DistributerStateMessage>(e.InputAddress); });
                        }));
                    });
                    services.AddSingleton<IDomainBusProducer, DomainBusProducer>();
                    services.AddHostedService<OrderBookDistributerService>();
                })
                .UseConsoleLifetime();

            await hostBuilder.RunConsoleAsync();
        }
    }
}