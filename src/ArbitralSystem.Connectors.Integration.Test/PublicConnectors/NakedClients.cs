
using Binance.Net;
using Binance.Net.Interfaces;
using Bittrex.Net;
using Bittrex.Net.Interfaces;
using CoinEx.Net;
using CoinEx.Net.Interfaces;
using Huobi.Net;
using Huobi.Net.Interfaces;
using Kraken.Net;
using Kraken.Net.Interfaces;
using Kucoin.Net;
using Kucoin.Net.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Connectors.Integration.PublicConnectors.Test
{
    [TestClass]
    public class NakedClients
    {
        private IBinanceClient _binanceClient;
        private IBittrexClient _bittrexClient;
        private ICoinExClient _coinExClient;
        private IHuobiClient _huobiClient;
        private IKrakenClient _krakenClient;
        private IKucoinClient _kucoinClient;

        [TestInitialize]
        public void Init()
        {
            _binanceClient = new BinanceClient();
            _bittrexClient = new BittrexClient();
            _coinExClient = new CoinExClient();
            _huobiClient = new HuobiClient();
            _krakenClient = new KrakenClient();
            _kucoinClient = new KucoinClient();
        }

        [TestMethod]
        public void TestClients()
        {
            _binanceClient.SetApiCredentials();
            _bittrexClient.SetApiCredentials();
            _coinExClient.SetApiCredentials();
            _huobiClient.SetApiCredentials();
            _krakenClient.SetApiCredentials();
            _kucoinClient.CreateAccount()
        }
    }
}