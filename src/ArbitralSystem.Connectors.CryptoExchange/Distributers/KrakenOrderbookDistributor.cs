using ArbitralSystem.Common.Converters;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Domain.MarketInfo;
using Kraken.Net;

namespace ArbitralSystem.Connectors.CryptoExchange.Distributers
{
    internal class KrakenOrderbookDistributor : BaseOrderBookDistributer<KrakenSymbolOrderBook>, IOrderbookDistributor
    {
        private const int DefaultOrderBookLimit = 100;
        private readonly IDistributerOptions _distributerOptions;

        public KrakenOrderbookDistributor(IDistributerOptions distributerOptions,
            IConverter converter,
            ILogger logger)
            : base(distributerOptions, converter, logger)
        {
            _distributerOptions = distributerOptions;
        }

        public override Exchange Exchange => Exchange.Kraken;

        protected override KrakenSymbolOrderBook CreateSymbolOrderBook(string symbol)
        {
            return new KrakenSymbolOrderBook(symbol, _distributerOptions.Limit ?? DefaultOrderBookLimit,
                new KrakenOrderBookOptions());
        }
    }
}