using System;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.PublicConnectors;
using ArbitralSystem.Domain.MarketInfo;
using JetBrains.Annotations;

namespace ArbitralSystem.Connectors.CryptoExchange
{
    public class CryptoExPublicConnectorFactory : IPublicConnectorFactory
    {
        private readonly ICoinExConnector _coinExConnector;
        private readonly IDtoConverter _converter;
        private readonly ILogger _logger;

        public CryptoExPublicConnectorFactory(ICoinExConnector coinExConnector,
            [NotNull] IDtoConverter converter,
            ILogger logger)
        {
            _coinExConnector = coinExConnector;
            _converter = converter;
            _logger = logger;
        }

        public IPublicConnector GetInstance(Exchange exchange)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return new BinancePublicConnector(_converter);

                case Exchange.Bittrex:
                    return new BittrexPublicConnector(_converter);

                case Exchange.CoinEx:
                    return new CoinExPublicConnector(_coinExConnector, _converter, _logger);

                case Exchange.Huobi:
                    return new HuobiPublicConnector(_converter);

                case Exchange.Kraken:
                    return new KrakenPublicConnector(_converter);

                case Exchange.Kucoin:
                    return new KucoinPublicConnector(_converter);

                default:
                    throw new NotSupportedException(
                        $"Not supported {Enum.GetName(typeof(Exchange), exchange)} exchange for {nameof(CryptoExOrderBookDistributerFactory)}");
            }
        }
    }
}