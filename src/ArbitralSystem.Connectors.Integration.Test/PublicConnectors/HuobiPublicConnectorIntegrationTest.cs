﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Connectors.CryptoExchange.PublicConnectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Connectors.Integration.PublicConnectors.Test
{
    [TestClass]
    public class HuobiPublicConnectorIntegrationTest
    {
        private const int _timeDelta = 5;
        private readonly IDtoConverter _dtoConverter;
        private readonly IPublicConnector _publicConnector;

        public HuobiPublicConnectorIntegrationTest()
        {
            _dtoConverter = new CryptoExchangeConverter();
            _publicConnector = new HuobiPublicConnector(_dtoConverter);
        }

        [TestMethod]
        public async Task GetServerTimeTest()
        {
            var serverTime = await _publicConnector.GetServerTime();

            var dateTimeNow = DateTime.UtcNow;
            var timeStampNow = TimeHelper.DateTimeToTimeStamp(dateTimeNow);

            Assert.AreEqual(serverTime, timeStampNow, _timeDelta);
        }

        [TestMethod]
        public async Task GetPairInfoTest()
        {
            var pairInfos = await _publicConnector.GetPairsInfo();

            if (!pairInfos.Any())
                throw new Exception("Empty pair info list from Huobi");

            var pairInfo = pairInfos.First();

            Assert.IsNotNull(pairInfo.BaseCurrency);
            Assert.IsNotNull(pairInfo.QuoteCurrency);
            Assert.IsNotNull(pairInfo.ExchangePairName);
            Assert.IsNotNull(pairInfo.UnificatedPairName);
        }
    }
}