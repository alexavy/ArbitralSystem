using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Domain.Distributers;
using CryptoExchange.Net.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Connectors.Test
{
    [TestClass]
    public class DistributerStatusConverterTest
    {
        private readonly IDtoConverter _dtoConverter;

        public DistributerStatusConverterTest()
        {
            _dtoConverter = new CryptoExchangeConverter();
        }

        [DataTestMethod]
        [DataRow(OrderBookStatus.Connecting)]
        [DataRow(OrderBookStatus.Disconnected)]
        [DataRow(OrderBookStatus.Synced)]
        [DataRow(OrderBookStatus.Syncing)]
        public void ConvertOrderBookStatusToDistributerStatus(OrderBookStatus rawStatus)
        {
            var targetStatus = _dtoConverter.Convert<OrderBookStatus, DistributerSyncStatus>(rawStatus);

            Assert.AreEqual((int) rawStatus, (int) targetStatus);
        }
    }
}