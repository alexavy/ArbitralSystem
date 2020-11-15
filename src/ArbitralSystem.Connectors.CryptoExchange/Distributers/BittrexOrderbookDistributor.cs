using System.Linq;
using ArbitralSystem.Common.Converters;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Domain.MarketInfo;
using Bittrex.Net;
using Bittrex.Net.Objects;

namespace ArbitralSystem.Connectors.CryptoExchange.Distributers
{
    internal class BittrexOrderbookDistributor : BaseOrderBookDistributer<BittrexSymbolOrderBook>, IOrderbookDistributor
    {
        private readonly IConverter _converter;
        private readonly ILogger _logger;

        public BittrexOrderbookDistributor(IDistributerOptions distributerOptions,
            IConverter converter,
            ILogger logger)
            : base(distributerOptions, converter, logger)
        {
            _converter = converter;
            _logger = logger;
        }

        public override Exchange Exchange => Exchange.Bittrex;

        protected override BittrexSymbolOrderBook CreateSymbolOrderBook(string symbol)
        {
            // Guys from Bittrex suddenly decided to switch currencies in pair!
            string switchedSymbol  = symbol.Split('-').Reverse().Aggregate((first, second) => $"{first}-{second}");
            return new BittrexSymbolOrderBook(switchedSymbol, 25 ,new BittrexOrderBookOptions());
        }
    }
}