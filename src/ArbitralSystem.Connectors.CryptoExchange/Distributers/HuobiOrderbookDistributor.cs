using ArbitralSystem.Common.Converters;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.Core.Types;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using Huobi.Net;
using System;
using System.Collections.Generic;
using System.Text;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.CryptoExchange.Distributers
{
    internal class HuobiOrderbookDistributor : BaseOrderBookDistributer<HuobiSymbolOrderBook>, IOrderbookDistributor
    {
        public HuobiOrderbookDistributor(IDistributerOptions distributerOptions,
                                     IConverter converter,
                                     ILogger logger)
                                     : base(distributerOptions, converter, logger)
        {
        }

        public override Exchange Exchange => Exchange.Huobi;

        protected override HuobiSymbolOrderBook CreateSymbolOrderBook(string symbol) =>
                    new HuobiSymbolOrderBook(symbol, new HuobiOrderBookOptions());
    }
}
