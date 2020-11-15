using ArbitralSystem.Common.Converters;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Domain.MarketInfo;
using Kucoin.Net;
using Kucoin.Net.Objects;

namespace ArbitralSystem.Connectors.CryptoExchange.Distributers
{
    internal class KucoinOrderbookDistributor : BaseOrderBookDistributer<KucoinSymbolOrderBook>, IOrderbookDistributor
    {
        public KucoinOrderbookDistributor(IDistributerOptions distributerOptions,
            IConverter converter,
            ILogger logger)
            : base(distributerOptions, converter, logger)
        {
        }

        public override Exchange Exchange => Exchange.Kucoin;

        protected override KucoinSymbolOrderBook CreateSymbolOrderBook(string symbol)
        {
            return new KucoinSymbolOrderBook(symbol, new KucoinOrderBookOptions());
        }
    }
}