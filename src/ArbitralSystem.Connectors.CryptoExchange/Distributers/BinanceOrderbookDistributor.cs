using System.Runtime.CompilerServices;
using ArbitralSystem.Common.Converters;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Domain.MarketInfo;
using Binance.Net;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot;

[assembly: InternalsVisibleTo("ArbitralSystem.Connectors.Integration.Test")]

namespace ArbitralSystem.Connectors.CryptoExchange.Distributers
{
    internal class BinanceOrderbookDistributor : BaseOrderBookDistributer<BinanceSymbolOrderBook>, IOrderbookDistributor
    {
        private const int DefaultOrderBookLimit = 20;
        private readonly IConverter _converter;
        private readonly IDistributerOptions _distributerOptions;
        private readonly ILogger _logger;

        public BinanceOrderbookDistributor(IDistributerOptions distributerOptions,
            IConverter converter,
            ILogger logger)
            : base(distributerOptions, converter, logger)
        {
            _distributerOptions = distributerOptions;
            _converter = converter;
            _logger = logger;
        }

        public override Exchange Exchange => Exchange.Binance;

        protected override BinanceSymbolOrderBook CreateSymbolOrderBook(string symbol)
        {
            return new BinanceSymbolOrderBook(symbol,
                new BinanceOrderBookOptions(_distributerOptions.Limit ?? DefaultOrderBookLimit));
        }
    }
}