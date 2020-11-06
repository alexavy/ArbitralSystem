﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Connectors.CryptoExchange.Distributers;
using ArbitralSystem.Connectors.CryptoExchange.PublicConnectors;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Connectors.Integration.Test.SingleOrderBookDistributer
{
    
    [TestClass]
    public class AllExchangesTest : BaseOrderBookDistributerTest
    {

        private readonly IOrderBookDistributerFactory _distributerFactory;
        private readonly IPublicConnectorFactory _connectorFactory;
        
        public AllExchangesTest()
        {
            _distributerFactory = CreateDistributerFactory();
            _connectorFactory = CreateConnectorFactory();
        }

        [DataTestMethod]
        [Ignore]
        [DataRow(Exchange.Binance,10)]
        [DataRow(Exchange.Bittrex,10)]
        [DataRow(Exchange.Huobi,10)]
        [DataRow(Exchange.Kraken,10)]
        [DataRow(Exchange.Kucoin,10)]
        [DataRow(Exchange.CoinEx,10)]
        public async Task AllExchangesDistributerTest(Exchange exchange ,int countOfPairs)
        {
            var publicConnector = _connectorFactory.GetInstance(exchange);

            var allPairs = await publicConnector.GetPairsInfo();
            Assert.IsTrue(allPairs.Any());
            
            var workPairs = allPairs.Take(countOfPairs);
            
            var counter = 0;
            foreach (var pairInfo in workPairs)
            {
                var orderbooks = new List<IOrderBook>();
                var distributer = _distributerFactory.GetInstance(exchange);
                distributer.OrderBookChanged += delegate(IOrderBook orderBook) { orderbooks.Add(orderBook); };
                var tokenSource = new CancellationTokenSource(TimeBeforeCancelDistribution);
                var orderBookPairInfo = new OrderBookPairInfo(exchange, pairInfo.ExchangePairName, pairInfo.UnificatedPairName);
                var distrTask = await distributer.StartDistributionAsync(orderBookPairInfo, tokenSource.Token);

                await distrTask;

                Assert.AreEqual(orderbooks.Any(), true);

                var orderbook = orderbooks.First();
                Assert.AreEqual(orderbook.Exchange, exchange);

                if (orderbook.Asks.Any())
                {
                    var ask = orderbook.Asks.First();
                    Assert.AreEqual(orderbook.BestAsk.Quantity, ask.Quantity);
                    Assert.AreEqual(orderbook.BestAsk.Price, ask.Price);
                }

                if (orderbook.Bids.Any())
                {
                    var bid = orderbook.Bids.First();
                    Assert.AreEqual(orderbook.BestBid.Quantity, bid.Quantity);
                    Assert.AreEqual(orderbook.BestBid.Price, bid.Price);
                }

                counter++;
            }
            Assert.AreEqual(workPairs.Count(), counter);
        }
        
        private IOrderBookDistributerFactory CreateDistributerFactory()
        {
            var options = new DistributerOptions {Frequency = 100};
            return  new CryptoExOrderBookDistributerFactory(options,
                DtoConverter,
                Logger);
        }

        private IPublicConnectorFactory CreateConnectorFactory()
        {
            var connectionInfo = new ExchangeConnectionInfo
            {
                Exchange = Exchange.CoinEx,
                BaseRestUrl = "https://api.coinex.com/"
            };

            IExchangeConnectionInfo[] connections = {connectionInfo};

            var coinExConnector = new CoinExConnector(connections, new EmptyLogger());

            return new CryptoExPublicConnectorFactory(coinExConnector, DtoConverter, new EmptyLogger());
        }
        
    }
}