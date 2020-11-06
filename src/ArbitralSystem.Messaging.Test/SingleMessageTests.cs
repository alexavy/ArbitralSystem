using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Domain.Distributers.Models;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Messaging.Commands;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Handlers;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Messaging.Test
{
    [TestClass]
    public class SingleMessageTests
    {
        private readonly IConnectionOptions _connectionOptions;
        private readonly IBusControlFactory _busControlFactory;
        public SingleMessageTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var connectionString = configuration.GetSection("RabbitMQ:Host")
                .Get<string>();
            
            _connectionOptions = new ConnectionOptions()
            {
                Host = connectionString
            };
            
            _busControlFactory = new BusControlFactory();
        }
        
        [TestMethod]
        public async Task OrderbookPriceEntriesTest()
        {
            IOrderbookPackageEntriesMessage receivedMessage = null;
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(_connectionOptions.Host);

                cfg.ReceiveEndpoint(Constants.Queues.OrderbooksStorageConsumer, endpoint =>
                {
                    endpoint.Handler<IOrderbookPackageEntriesMessage>(async context =>
                    {
                        receivedMessage = context.Message;
                        await Console.Out.WriteLineAsync($"Received: {context.Message}");
                    });
                });
            });

            await busControl.StartAsync();
            
            var message = CreateTestMessage();
            var command = new SingleSaveOrderbookPackageEntriesCommand(message);
            var handler = new OrderbookPriceEntriesHandler(_connectionOptions, _busControlFactory);
            await handler.HandleAsync(command);
            
            Thread.Sleep(1000);
            await busControl.StopAsync();
            Assert.IsNotNull(receivedMessage);
        }

        private IOrderbookPackageEntriesMessage CreateTestMessage()
        {
            var entry = new OrderbookPriceEntry()
            {
                Exchange = Exchange.Binance,
                Direction = Direction.Buy,
                Symbol = "ETH/BTC",
                Price = 1,
                Quantity = 1
            };
            return new OrderbookPackageEntriesMessage(new List<IOrderbookPriceEntry>() {entry});
        }
    }
}