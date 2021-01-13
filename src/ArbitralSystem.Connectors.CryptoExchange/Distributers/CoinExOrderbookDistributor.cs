using ArbitralSystem.Common.Converters;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Domain.MarketInfo;
using CoinEx.Net;
using CoinEx.Net.Objects;

namespace ArbitralSystem.Connectors.CryptoExchange.Distributers
{
    internal class CoinExOrderbookDistributor : BaseOrderBookDistributer<CoinExSymbolOrderBook>, IOrderbookDistributor
    {
        public CoinExOrderbookDistributor(IDistributerOptions distributerOptions,
            IConverter converter,
            ILogger logger)
            : base(distributerOptions, converter, logger)
        {
        }

        public override Exchange Exchange => Exchange.CoinEx;

        protected override CoinExSymbolOrderBook CreateSymbolOrderBook(string symbol)
        {
            return new CoinExSymbolOrderBook(symbol, new CoinExOrderBookOptions());
        }
    }
}