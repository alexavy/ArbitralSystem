
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Distributer.PairInfoDistributerService.Options;
using ArbitralSystem.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Service.Core.Messaging;
using ArbitralSystem.Storage.RemoteCacheStorage;

namespace ArbitralSystem.Distributer.PairInfoDistributerService.Test
{

    [TestClass]
    public class ControlServiceMoreThenTwoExchangesTest : BakedPairs
    {
        private readonly IPairInfoDistributerOptions _pairInfoDistributerOptions;

        public ControlServiceMoreThenTwoExchangesTest()
        {
            _pairInfoDistributerOptions = new PairInfoDistributerOptions()
            {
                SiftType = SiftType.ListedMoreThenTwoExchanges
            };
        }

        [TestMethod]
        public async Task OnlyOneExchange()
        {

            var mockConnector = new Mock<IPublicConnector>();
            mockConnector.Setup(o => o.GetPairsInfo()).ReturnsAsync(binance_3_ETHBTC_ETHLTC_ETHBNB);

            var mockConnectorFactory = new Mock<IPublicConnectorFactory>();
            mockConnectorFactory.Setup(o=>o.GetInstance(It.Is<Exchange>(m => m == Exchange.Binance))).Returns(mockConnector.Object);

            var mockStorage = new Mock<IPairCacheStorage>();

            int messagesPublished = 0;
            var mockBusProducer = new Mock<IDomainBusProducer>();
            mockBusProducer.Setup(o => o.PublishAsync(It.IsAny<IPairInfo>())).Callback(() => messagesPublished++);

            ControlService controlService = new ControlService(mockConnectorFactory.Object,
                                                _pairInfoDistributerOptions,
                                                mockStorage.Object,
                                                mockBusProducer.Object,
                                                new EmptyLogger());

            await controlService.StartAsync(CancellationToken.None);

            Assert.AreEqual(messagesPublished, 0);
        }
        [TestMethod]
        public async Task TwoExchangesAndOneSimilarPair()
        {
            var binanceMmockConnector = new Mock<IPublicConnector>();
            binanceMmockConnector.Setup(o => o.GetPairsInfo()).ReturnsAsync(binance_3_ETHBTC_ETHLTC_ETHBNB);

            var bittrexMmockConnector = new Mock<IPublicConnector>();
            bittrexMmockConnector.Setup(o => o.GetPairsInfo()).ReturnsAsync(binance_1_ETHBTC);

            var mockConnectorFactory = new Mock<IPublicConnectorFactory>();
            mockConnectorFactory.Setup(o => o.GetInstance(It.Is<Exchange>(m => m == Exchange.Binance))).Returns(binanceMmockConnector.Object);
            mockConnectorFactory.Setup(o => o.GetInstance(It.Is<Exchange>(m => m == Exchange.Bittrex))).Returns(bittrexMmockConnector.Object);

            var mockStorage = new Mock<IPairCacheStorage>();

            int messagesPublished = 0;
            var mockBusProducer = new Mock<IDomainBusProducer>();
            mockBusProducer.Setup(o => o.PublishAsync(It.IsAny<IPairInfo>())).Callback(() => messagesPublished++);

            ControlService controlService = new ControlService(mockConnectorFactory.Object,
                                                _pairInfoDistributerOptions,
                                                mockStorage.Object,
                                                mockBusProducer.Object,
                                                new EmptyLogger());

            await controlService.StartAsync(CancellationToken.None);

            Assert.AreEqual(messagesPublished, 1);
        }
        [TestMethod]
        public async Task TwoExchangesAndTwoSimilarPairs()
        {
            var binanceMmockConnector = new Mock<IPublicConnector>();
            binanceMmockConnector.Setup(o => o.GetPairsInfo()).ReturnsAsync(binance_3_ETHBTC_ETHLTC_ETHBNB);

            var bittrexMmockConnector = new Mock<IPublicConnector>();
            bittrexMmockConnector.Setup(o => o.GetPairsInfo()).ReturnsAsync(binance_2_ETHBTC_ETHLTC);

            var mockConnectorFactory = new Mock<IPublicConnectorFactory>();
            mockConnectorFactory.Setup(o => o.GetInstance(It.Is<Exchange>(m => m == Exchange.Binance))).Returns(binanceMmockConnector.Object);
            mockConnectorFactory.Setup(o => o.GetInstance(It.Is<Exchange>(m => m == Exchange.Bittrex))).Returns(bittrexMmockConnector.Object);

            var mockStorage = new Mock<IPairCacheStorage>();

            int messagesPublished = 0;
            var mockBusProducer = new Mock<IDomainBusProducer>();
            mockBusProducer.Setup(o => o.PublishAsync(It.IsAny<IPairInfo>())).Callback(() => messagesPublished++);

            ControlService controlService = new ControlService(mockConnectorFactory.Object,
                                                _pairInfoDistributerOptions,
                                                mockStorage.Object,
                                                mockBusProducer.Object,
                                                new EmptyLogger());

            await controlService.StartAsync(CancellationToken.None);

            Assert.AreEqual(messagesPublished, 2);
        }
        [TestMethod]
        public async Task TwoExchangesAndTwoSimilarPairsGettedFromCache()
        {
            var mockConnectorFactory = new Mock<IPublicConnectorFactory>();

            var mockStorage = new Mock<IPairCacheStorage>();
            mockStorage.Setup(o => o.GetAllPairsAsync(It.Is<Exchange>(m => m == Exchange.Binance))).ReturnsAsync(binance_3_ETHBTC_ETHLTC_ETHBNB);
            mockStorage.Setup(o => o.GetAllPairsAsync(It.Is<Exchange>(m => m == Exchange.Bittrex))).ReturnsAsync(binance_2_ETHBTC_ETHLTC);

            int messagesPublished = 0;
            var mockBusProducer = new Mock<IDomainBusProducer>();
            mockBusProducer.Setup(o => o.PublishAsync(It.IsAny<IPairInfo>())).Callback(() => messagesPublished++);

            ControlService controlService = new ControlService(mockConnectorFactory.Object,
                                                _pairInfoDistributerOptions,
                                                mockStorage.Object,
                                                mockBusProducer.Object,
                                                new EmptyLogger());

            await controlService.StartAsync(CancellationToken.None);

            Assert.AreEqual(messagesPublished, 2);
        }
    }
}
