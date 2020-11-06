using System;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Connectors.CryptoExchange.PublicConnectors;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Connectors.Integration.PublicConnectors.Test
{
    [TestClass]
    public class CoinExPublicConnectorIntegrationTest
    {
        private const int _timeDelta = 5;
        private readonly IDtoConverter _dtoConverter;
        private readonly IPublicConnector _publicConnector;

        public CoinExPublicConnectorIntegrationTest()
        {
            _dtoConverter = new CryptoExchangeConverter();

            var connectionInfo = new ExchangeConnectionInfo
            {
                Exchange = Exchange.CoinEx,
                BaseRestUrl = "https://api.coinex.com/"
            };

            IExchangeConnectionInfo[] connections = {connectionInfo};

            var coinxConnector = new CoinExConnector(connections, new EmptyLogger());
            _publicConnector = new CoinExPublicConnector(coinxConnector, _dtoConverter, new EmptyLogger());
        }


        //[TestMethod]
        //public async Task GetServerTimeTest()
        //{
        //    var serverTime = await _publicConnector.GetServerTime();

        //    var dateTimeNow = DateTime.UtcNow;
        //    var timeStampNow = TimeHelper.DateTimeToTimeStamp(dateTimeNow);

        //    Assert.AreEqual(serverTime, timeStampNow, _timeDelta);
        //}

        [TestMethod]
        public async Task GetPairInfoTest()
        {
            var pairInfos = await _publicConnector.GetPairsInfo();

            if (!pairInfos.Any())
                throw new Exception("Empty pair info list from Coinex");

            var pairInfo = pairInfos.First();

            Assert.IsNotNull(pairInfo.BaseCurrency);
            Assert.IsNotNull(pairInfo.QuoteCurrency);
            Assert.IsNotNull(pairInfo.ExchangePairName);
            Assert.IsNotNull(pairInfo.UnificatedPairName);
        }
    }
}