using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Options;
using ArbitralSystem.Service.Core;
using ArbitralSystem.Storage.MarketInfoStorageService.Consumers;
using ArbitralSystem.Storage.MarketInfoStorageService.Common;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Interfaces;
using ArbitralSystem.Storage.MarketInfoStorageService.Extensions;
using ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Repositories;
using AutoMapper;
using GreenPipes;
using MassTransit;
using MassTransit.Context;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Storage.MarketInfoStorageService
{
    internal class Program
    {
        private static readonly string EnvironmentBuildName = Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT");
        private static readonly string ApplicationName = "MarketInfoStorage";

        private static string DomainAssembly => "ArbitralSystem.Storage.MarketInfoStorageService.Domain";
        private static string PersistenceAssembly => "ArbitralSystem.Storage.MarketInfoStorageService.Persistence";

        private static async Task Main(string[] args)
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
                await MarketInfoStorageService(args, configuration, loggerWrapper);
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

        static async Task MarketInfoStorageService(string[] args, IConfigurationRoot configuration, ILogger loggerWrapper)
        {
            IConnectionOptions mqOptions = configuration.GetSection("RabbitMQ:Connection")
                .Get<ConnectionOptions>();

            IServiceStorageOptions serviceStorageOptions = configuration.GetSection("MarketInfoStorageService:Options")
                .Get<ServiceStorageOptions>();

            string databaseConnection = configuration.GetConnectionString("DefaultConnection");

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
                mqOptions,
                serviceStorageOptions,
                settingsLogger.HidePassword(databaseConnection, RegexPasswordPatterns.Database));

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) => { configApp.AddConfiguration(configuration); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(loggerWrapper);
                    services.AddSingleton(configuration);
                    services.AddSingleton(serviceStorageOptions);
                    services.AddMediatR(AppDomain.CurrentDomain.Load(DomainAssembly),AppDomain.CurrentDomain.Load(PersistenceAssembly));
                    services.AddAutoMapper(Assembly.GetExecutingAssembly(),AppDomain.CurrentDomain.Load(PersistenceAssembly) );
                    
                    services.AddScoped<IOrderBooksRepository, OrderBooksRepository>();
                    services.AddScoped<IDistributerStatesRepository, DistributerStatesRepository>();
                    
                    services.AddArbitralSystemDbContext(databaseConnection);

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<OrderBookConsumer>();
                        x.AddConsumer<DistributerStateConsumer>();
                        MessageCorrelation.UseCorrelationId<IOrderBookPackageMessage>(o => o.CorrelationId);
                        MessageCorrelation.UseCorrelationId<IDistributerStateMessage>(o => o.CorrelationId);

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            cfg.Host(new Uri(mqOptions.Host), h => { });

                            cfg.ReceiveEndpoint(Constants.Queues.OrderbooksStorageConsumer, e =>
                            {
                                e.PrefetchCount = serviceStorageOptions.PrefetchCount;
                                e.ConfigureConsumer<OrderBookConsumer>(provider);
                                e.UseMessageRetry(o => o.Interval(10, TimeSpan.FromSeconds(1)));
                            });

                            cfg.ReceiveEndpoint(Constants.Queues.DistributerConnectionStatesConsumer, e =>
                            {
                                e.PrefetchCount = 1; // maybe better to save state in multiple tasks
                                e.ConfigureConsumer<DistributerStateConsumer>(provider);
                                e.UseMessageRetry(o => o.Interval(10, TimeSpan.FromSeconds(1)));
                            });
                        }));
                    });
                    services.AddHostedService<StorageService>();
                })
                .UseConsoleLifetime();


            await hostBuilder.RunConsoleAsync();
        }

    }
}