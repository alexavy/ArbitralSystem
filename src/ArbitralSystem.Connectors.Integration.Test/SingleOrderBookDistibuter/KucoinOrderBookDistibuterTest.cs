﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Connectors.CryptoExchange.Distributers;
using ArbitralSystem.Connectors.CryptoExchange.Models;
using ArbitralSystem.Domain.MarketInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Connectors.Integration.Test.SingleOrderBookDistributer
{
    [TestClass]
    public class KucoinOrderBookDistributerTest : BaseOrderBookDistributerTest
    {
        [TestMethod]
        public async Task KucoinDistributionDefaultOptions()
        {
            var options = new DistributerOptions {Frequency = 100};

            var orderbooks = new List<IOrderBook>();

            var binanceDistributer = new KucoinOrderbookDistributor(options, DtoConverter, Logger);
            binanceDistributer.OrderBookChanged += delegate(IOrderBook orderBook) { orderbooks.Add(orderBook); };
            var tokenSource = new CancellationTokenSource(TimeBeforeCancelDistribution);

            var pairInfo = new PairInfo
            {
                ExchangePairName = "ETH-BTC",
                BaseCurrency = "ETH",
                QuoteCurrency = "BTC"
            };
            var orderBookPairInfo = new OrderBookPairInfo(Exchange.Kucoin, pairInfo.ExchangePairName, pairInfo.UnificatedPairName);
            var distrTask = await binanceDistributer.StartDistributionAsync(orderBookPairInfo, tokenSource.Token);

            var tokenForTaskSource = new CancellationTokenSource(TimeBeforeCancelTask);

            distrTask.Wait(tokenForTaskSource.Token);

            Assert.AreEqual(distrTask.Status, TaskStatus.RanToCompletion);
            Assert.AreEqual(orderbooks.Any(), true);

            var orderbook = orderbooks.First();
            Assert.AreEqual(orderbook.Exchange, Exchange.Kucoin);

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
        }
    }
}